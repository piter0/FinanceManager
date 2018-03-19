using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;

namespace FinanceManager.WebUI.Controllers
{
    public class ExpenseController : Controller
    {
        private IExpenseRepository repository;

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            repository = expenseRepository;
        }

        public ActionResult Index(string date)
        {
            if(date == null)
            {
                return View(repository.Expenses);
            }
            else
            {
                var expenses = repository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

                return View(expenses);
            }
            
        }

        public ViewResult Edit(int expenseID)
        {
            Expense expense = repository.Expenses.FirstOrDefault(i => i.ExpenseID == expenseID);

            return View(expense);
        }

        [HttpPost]
        public ActionResult Edit(Expense expense)
        {
            if (ModelState.IsValid)
            {
                repository.Save(expense);
                TempData["message"] = string.Format("Zaktualizowano {0}", expense.Description);
                return RedirectToAction("Index");
            }
            else
            {
                return View(expense);
            }
        }

        public ViewResult Create()
        {
            return View("Create", new Expense());
        }

        [HttpPost]
        public ActionResult Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                repository.Save(expense);
                TempData["message"] = string.Format("Utworzono {0}", expense.Description);
                return RedirectToAction("Index");
            }
            else
            {
                return View(expense);
            }
        }

        [HttpPost]
        public ActionResult Delete(int expenseID)
        {
            Expense deletedExpense = repository.Delete(expenseID);

            if (deletedExpense != null)
            {
                TempData["message"] = string.Format("Usunięto {0} z dnia {1}", deletedExpense.Description, deletedExpense.Date.ToString("d"));
            }

            return RedirectToAction("Index");
        }
    }
}