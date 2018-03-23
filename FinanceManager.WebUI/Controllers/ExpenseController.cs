using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;
using PagedList;

namespace FinanceManager.WebUI.Controllers
{
    public class ExpenseController : Controller
    {
        private IExpenseRepository repository;

        private ICategoryRepository categoryRepository;

        public ExpenseController(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository)
        {
            repository = expenseRepository;
            this.categoryRepository = categoryRepository;
        }

        public ActionResult Index(string date, string sortBy, string currentSort, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewBag.dateParam = sortBy == "date" ? "dateDesc" : "date";
            ViewBag.sumParam = sortBy == "sum" ? "sumDesc" : "sum";
            ViewBag.categoryParam = sortBy == "category" ? "categoryDesc" : "category";
            
            if (date == null)
            {
                date = DateTime.Now.ToString("MM-yyyy");
                ViewBag.selectedDate = date;

                var expenses = repository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

                return View(expenses.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var expenses = repository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

                ViewBag.selectedDate = date;

                switch (sortBy)
                { //
                    case "date":
                        expenses = expenses.OrderBy(x => x.Date);
                        ViewBag.currentSort = "date";
                        break;
                    case "dateDesc":
                        expenses = expenses.OrderByDescending(x => x.Date);
                        ViewBag.currentSort = "dateDesc";
                        break;
                    case "sum":
                        expenses = expenses.OrderBy(x => x.Price);
                        ViewBag.currentSort = "sum";
                        break;
                    case "sumDesc":
                        expenses = expenses.OrderByDescending(x => x.Price);
                        ViewBag.currentSort = "sumDesc";
                        break;
                    case "category":
                        expenses = expenses.OrderBy(x => x.Category);
                        ViewBag.currentSort = "category";
                        break;
                    case "categoryDesc":
                        expenses = expenses.OrderByDescending(x => x.Category);
                        ViewBag.currentSort = "categoryDesc";
                        break;
                    default:
                        expenses = expenses.OrderBy(x => x.ExpenseID);
                        break;
                }

                return View(expenses.ToPagedList(pageNumber, pageSize));
            }
        }

        public ViewResult Edit(int expenseID)
        {
            Expense expense = repository.Expenses.FirstOrDefault(i => i.ExpenseID == expenseID);

            ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Expense"), "Name", "Name");

            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Expense"), "Name", "Name");
                return View(expense);
            }
        }

        public ViewResult Create()
        {
            ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Expense"), "Name", "Name");
            return View("Create", new Expense());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Expense"), "Name", "Name");
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