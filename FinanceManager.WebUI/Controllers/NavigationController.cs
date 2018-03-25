using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;

namespace FinanceManager.WebUI.Controllers
{
    public class NavigationController : Controller
    {
        private IExpenseRepository expenseRepository;
        private IIncomeRepository incomeRepository;
        private ISavingRepository savingRepository;

        public NavigationController(IExpenseRepository eRepository, IIncomeRepository iRepository, ISavingRepository sRepository)
        {
            expenseRepository = eRepository;
            incomeRepository = iRepository;
            savingRepository = sRepository;
        }

        public PartialViewResult Index(string type, string date = null)
        {
            IEnumerable<string> navigationMonths;

            if(type == "Summary")
            {
                navigationMonths = incomeRepository.Incomes.Select(x => x.Date.ToString("MM-yyy")).
                                    Concat(savingRepository.Savings.Select(x => x.Date.ToString("MM-yyy"))).
                                    Concat(expenseRepository.Expenses.Select(x => x.Date.ToString("MM-yyy"))).
                                    Distinct().OrderByDescending(x => x);
            }
            else if (type == "Income")
            {
                navigationMonths = incomeRepository.Incomes.Select(x => x.Date.ToString("MM-yyy")).Distinct().OrderByDescending(x => x);
            }
            else if(type == "Saving")
            {
                navigationMonths = savingRepository.Savings.Select(x => x.Date.ToString("MM-yyy")).Distinct().OrderByDescending(x => x);
            }
            else
            {
                navigationMonths = expenseRepository.Expenses.Select(x => x.Date.ToString("MM-yyy")).Distinct().OrderByDescending(x => x);
            }
            
            ViewBag.selectedDate = date;
            ViewBag.type = type;

            return PartialView(navigationMonths);
        }
    }
}