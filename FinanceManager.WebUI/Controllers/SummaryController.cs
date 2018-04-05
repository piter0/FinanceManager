using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;
using FinanceManager.WebUI.Extensions;

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
            ViewBag.controllerName = "Summary";// Information for navigation controller

            var ExpensesByDate = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
            var IncomesByDate = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
            var SavingsByDate = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

            decimal ExpensesSum = ExpensesByDate.Sum(x => x.Price);
            decimal IncomesSum = IncomesByDate.Sum(x => x.Price);
            decimal SavingsSum = SavingsByDate.Sum(x => x.Price);

            int ExpensesRecords = ExpensesByDate.Count();
            int IncomesRecords = IncomesByDate.Count();
            int SavingsRecords = SavingsByDate.Count();

            double Incomes = (IncomesSum > 0) ? 100 : 0;
            double ExpensesToIncomes;
            double SavingToIncomes;

            if (Incomes > 0 && ExpensesSum > 0)
            {
                ExpensesToIncomes = Math.Round((double)(ExpensesSum / IncomesSum) * 100, 2);
            }
            else if (Incomes == 0 && ExpensesSum > 0)
            {
                ExpensesToIncomes = double.PositiveInfinity;
            }
            else
            {
                ExpensesToIncomes = 0.0;
            }

            if (Incomes > 0 && SavingsSum > 0)
            {
                SavingToIncomes = Math.Round((double)(SavingsSum / IncomesSum) * 100, 2);
            }
            else if (Incomes == 0 && SavingsSum > 0)
            {
                SavingToIncomes = double.PositiveInfinity;
            }
            else
            {
                SavingToIncomes = 0.0;
            }

            Summary Summary = new Summary
            {
                ExpensesSum = ExpensesSum,
                IncomesSum = IncomesSum,
                SavingsSum = SavingsSum,
                ExpensesToIncomes = ExpensesToIncomes,
                Incomes = Incomes,
                SavingsToIncomes = SavingToIncomes,
                ExpensesRecords = ExpensesRecords,
                IncomesRecords = IncomesRecords,
                SavingsRecords = SavingsRecords,
                Date = date
            };

            ViewBag.summaryMessage = (Summary.IncomesSum > Summary.ExpensesSum) ?
                                     "Brawo! Twoje miesięczne dochody są większe od wydatków." :
                                     "Niestety, w tym miesiącu Twoje wydatki przewyższyły dochody.";

            return View(Summary);
        }

        public PartialViewResult ExpenseDetails(string date)
        {
            Details Details = new Details
            {
                Date = date,
                DetailedList = expenseRepository.GetDetailedList(date),
                CategorySum = expenseRepository.TotalExpensesSum(date),
                Coniugation = "wydatków"
            };

            if (Details.CategorySum > 0)
            {
                return PartialView("Details", Details);
            }
            else
            {
                ViewBag.noRecords = "Brak wydatków w tym miesiącu!";
                return PartialView("NoRecords");
            }

        }
        //    List<string> categoryType;

        //    if (type == "Expense") // Compute the sum of each expense category in given date
        //    {
        //        Details.Coniugation = ;

        //        var categories = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
        //        decimal categorySum = categories.Sum(x => x.Price);

        //        if (categorySum > 0)
        //        {
        //            Details.CategorySum = categorySum;
        //            categoryType = categories.Select(x => x.Category).Distinct().ToList();

        //            foreach (var categoryName in categoryType)
        //            {
        //                Details.DetailedList.Add(new Tuple<string, decimal, int, double>(categoryName.ToString(), expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Count(), Math.Round((((double)expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price) / (double)expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price)) * 100), 2)));
        //            }

        //            return PartialView(Details);
        //        }
        //        else
        //        {
        //            ViewBag.noRecords = "Brak wydatków w tym miesiącu!";
        //            return PartialView("NoRecords");
        //        }

        //    }
        //    else if (type == "Income")
        //    {
        //        Details.Coniugation = "przychodów";

        //        var categories = incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
        //        decimal categorySum = categories.Sum(x => x.Price);

        //        if (categorySum > 0)
        //        {
        //            Details.CategorySum = categorySum;
        //            categoryType = categories.Select(x => x.Category).Distinct().ToList();

        //            foreach (var categoryName in categoryType)
        //            {
        //                Details.DetailedList.Add(new Tuple<string, decimal, int, double>(categoryName.ToString(), incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Count(), Math.Round((((double)incomeRepository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price) / (double)categorySum) * 100), 2)));
        //            }

        //            return PartialView(Details);
        //        }
        //        else
        //        {
        //            ViewBag.noRecords = "Brak przychodów w tym miesiącu!";
        //            return PartialView("NoRecords");
        //        }
        //    }
        //    else
        //    {
        //        Details.Coniugation = "oszczędności";

        //        var categories = savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date));
        //        decimal categorySum = categories.Sum(x => x.Price);

        //        if (categorySum > 0)
        //        {
        //            Details.CategorySum = categorySum;
        //            categoryType = categories.Select(x => x.Category).Distinct().ToList();

        //            foreach (var categoryName in categoryType)
        //            {
        //                Details.DetailedList.Add(new Tuple<string, decimal, int, double>(categoryName.ToString(), savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Count(), Math.Round((((double)savingRepository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price) / (double)categorySum) * 100), 2)));
        //            }

        //            return PartialView(Details);
        //        }
        //        else
        //        {
        //            ViewBag.noRecords = "Brak oszczędności w tym miesiącu!";
        //            return PartialView("NoRecords");
        //        }
        //    }
        //}
    }
}