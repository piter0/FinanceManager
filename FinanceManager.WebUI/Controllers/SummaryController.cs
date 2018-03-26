using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;

namespace FinanceManager.WebUI.Controllers
{
    public class SummaryController : Controller
    {
        private IExpenseRepository expenseRepository;
        private IIncomeRepository incomeRepository;
        private ISavingRepository savingRepository;

        public SummaryController(IExpenseRepository expenseRepository, IIncomeRepository incomeRepository, ISavingRepository savingRepository)
        {
            this.expenseRepository = expenseRepository;
            this.incomeRepository = incomeRepository;
            this.savingRepository = savingRepository;
        }

        public ViewResult Index()
        {
            ViewBag.Summary = "Z menu po lewej wybierz miesiąc, z którego chcesz zobaczyć podsumowanie";
            ViewBag.type = "Summary";
            return View();
        }

        public ViewResult Show(string date)
        {
            Summary Summary = new Summary();
            {
                Summary.ExpensesSum = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                Summary.IncomesSum = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                Summary.SavingsSum = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                Summary.Date = date;
            };
            return View(Summary);
        }

        public ViewResult Details(string date, string type)
        {
            Details Details = new Details();
            {
                Details.Date = date;
                Details.DetailedList = new List<Tuple<string, decimal, double>>();
                Details.CategorySum = 0;
                Details.Coniugation = string.Empty;
            };

            List<string> categoryType;

            if (type == "Expense")
            {
                Details.Coniugation = "wydatków";
                decimal categorySum = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                Details.CategorySum = categorySum;
                categoryType = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Select(x => x.Category).Distinct().ToList();

                foreach (var categoryName in categoryType)
                {
                    Details.DetailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)categorySum)*100), 2)));
                }

                return View(Details);
            }
            else if (type == "Income")
            {
                Details.Coniugation = "przychodów";
                decimal categorySum = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                Details.CategorySum = categorySum;
                categoryType = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Select(x => x.Category).Distinct().ToList();

                foreach (var categoryName in categoryType)
                {
                    Details.DetailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)categorySum)*100), 2)));
                }

                return View(Details);
            }
            else
            {
                Details.Coniugation = "oszczędności";
                decimal categorySum = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                Details.CategorySum = categorySum;
                categoryType = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Select(x => x.Category).Distinct().ToList();

                foreach (var categoryName in categoryType)
                {
                    Details.DetailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)categorySum)*100), 2)));
                }

                return View(Details);
            }
        }
    }
}