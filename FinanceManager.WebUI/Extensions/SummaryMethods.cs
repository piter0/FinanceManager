using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinanceManager.Domain.Abstract;

namespace FinanceManager.WebUI.Extensions
{
    public static class SummaryMethods
    {
        public static List<Tuple<string, decimal, int, double>> GetDetailedList(this IExpenseRepository expenseRepository, string date)
        {
            var detailedList = new List<Tuple<string, decimal, int, double>>();

            var categoryType = expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Select(x => x.Category).Distinct().ToList();

            foreach (var categoryName in categoryType)
            {
                detailedList.Add(new Tuple<string, decimal, int, double>(categoryName.ToString(), expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price), expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Count(), Math.Round((((double)expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date) && x.Category == categoryName).Sum(x => x.Price) / (double)expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price)) * 100), 2)));
            }

            return detailedList;
        }

        public static decimal TotalExpensesSum(this IExpenseRepository expenseRepository, string date)
        {
            return expenseRepository.Expenses.Where(x => x.Date.ToString("MM-yyyy").Equals(date)).Sum(x => x.Price);
        }
    }
}