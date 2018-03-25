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
            ViewBag.date = date;
            List<string> categoryType;
            List<Tuple<string, decimal, double>> detailedList;

            if (type == "Expense")
            {
                ViewBag.type = "wydatków";
                decimal totalSum = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                ViewBag.totalSum = totalSum;
                categoryType = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Select(x => x.Category).Distinct().ToList();

                detailedList = new List<Tuple<string, decimal, double>>();

                foreach (var categoryName in categoryType)
                {
                    detailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)totalSum)*100), 2)));
                }

                return View(detailedList);
            }
            else if (type == "Income")
            {
                ViewBag.type = "dochodów";
                decimal totalSum = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                ViewBag.totalSum = totalSum;
                categoryType = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Select(x => x.Category).Distinct().ToList();

                detailedList = new List<Tuple<string, decimal, double>>();

                foreach (var categoryName in categoryType)
                {
                    detailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)totalSum)*100), 2)));
                }

                return View(detailedList);
            }
            else
            {
                ViewBag.type = "oszczędności";
                decimal totalSum = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
                ViewBag.totalSum = totalSum;
                categoryType = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Select(x => x.Category).Distinct().ToList();

                detailedList = new List<Tuple<string, decimal, double>>();

                foreach (var categoryName in categoryType)
                {
                    detailedList.Add(new Tuple<string, decimal, double>(categoryName.ToString(), savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), Math.Round((((double)savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price)/(double)totalSum)*100), 2)));
                }

                return View(detailedList);
            }
        }
    }
}