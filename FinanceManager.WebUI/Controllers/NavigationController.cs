using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;

namespace FinanceManager.WebUI.Controllers
{
    public class NavigationController : Controller
    {
        private IExpenseRepository repository;

        public NavigationController(IExpenseRepository expenseRepository)
        {
            repository = expenseRepository;
        }

        public PartialViewResult Index(string date = null)
        {
            IEnumerable<string> navigationMonths = repository.Expenses.Select(x => x.Date.ToString("MM-yyy")).Distinct().OrderByDescending(x => x);

            ViewBag.SelectedDate = date;

            return PartialView(navigationMonths);
        }
    }
}