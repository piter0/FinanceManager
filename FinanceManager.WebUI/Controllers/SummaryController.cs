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

        public ViewResult Index(string date)
        {
            date = string.IsNullOrEmpty(date) ? date = DateTime.Now.ToString("MM-yyyy") : date;
            ViewBag.type = "Summary";// Information for navigation controller

            Summary Summary = new Summary
            {
                ExpensesSum = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price),
                IncomesSum = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price),
                SavingsSum = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price),
                Date = date
            };
            return View(Summary);
        }

        public ViewResult Details(string date, string type)
        {
            Details Details = new Details
            {
                Date = date,
                DetailedList = new List<Tuple<string, decimal, double>>(),
                CategorySum = 0,
                Coniugation = string.Empty
            };

            List<string> categoryType;

            if (type == "Expense") // Compute the sum of each expense category in given date
            {
                Details.Coniugation = "wydatków";

                var categories = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
                decimal categorySum = categories.Sum(x => x.Price);
                Details.CategorySum = categorySum;
                categoryType = categories.Select(x => x.Category).Distinct().ToList();

                foreach (var categoryName in categoryType)
                {
                    Details.DetailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)categorySum)*100), 2)));
                }

                return View(Details);
            }
            else if (type == "Income")
            {
                Details.Coniugation = "przychodów";

                var categories = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
                decimal categorySum = categories.Sum(x => x.Price);
                Details.CategorySum = categorySum;
                categoryType = categories.Select(x => x.Category).Distinct().ToList();

                foreach (var categoryName in categoryType)
                {
                    Details.DetailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)categorySum)*100), 2)));
                }

                return View(Details);
            }
            else
            {
                Details.Coniugation = "oszczędności";

                var categories = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
                decimal categorySum = categories.Sum(x => x.Price);
                Details.CategorySum = categorySum;
                categoryType = categories.Select(x => x.Category).Distinct().ToList();

                foreach (var categoryName in categoryType)
                {
                    Details.DetailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)categorySum)*100), 2)));
                }

                return View(Details);
            }
        }
    }
}