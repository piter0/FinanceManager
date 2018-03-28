using System;
using System.Linq;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;
using PagedList;

namespace FinanceManager.WebUI.Controllers
{
    public class IncomeController : Controller
    {
        private IIncomeRepository repository;

        private ICategoryRepository categoryRepository;

        public IncomeController(IIncomeRepository incomeRepository, ICategoryRepository categoryRepository)
        {
            repository = incomeRepository;
            this.categoryRepository = categoryRepository;
        }

        public ActionResult Index(string date, string sortBy, string currentSort, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewBag.dateParam = sortBy == "date" ? "dateDesc" : "date";
            ViewBag.sumParam = sortBy == "sum" ? "sumDesc" : "sum";
            ViewBag.categoryParam = sortBy == "category" ? "categoryDesc" : "category";
            ViewBag.controllerName = "Income"; //information for Navigation controller

            if (date == null)
            {
                date = DateTime.Now.ToString("MM-yyyy");
                ViewBag.selectedDate = date;

                var incomes = repository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

                return View(incomes.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var incomes = repository.Incomes.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

                ViewBag.selectedDate = date;

                switch (sortBy)
                { //
                    case "date":
                        incomes = incomes.OrderBy(x => x.Date);
                        ViewBag.currentSort = "date";
                        break;
                    case "dateDesc":
                        incomes = incomes.OrderByDescending(x => x.Date);
                        ViewBag.currentSort = "dateDesc";
                        break;
                    case "sum":
                        incomes = incomes.OrderBy(x => x.Price);
                        ViewBag.currentSort = "sum";
                        break;
                    case "sumDesc":
                        incomes = incomes.OrderByDescending(x => x.Price);
                        ViewBag.currentSort = "sumDesc";
                        break;
                    case "category":
                        incomes = incomes.OrderBy(x => x.Category);
                        ViewBag.currentSort = "category";
                        break;
                    case "categoryDesc":
                        incomes = incomes.OrderByDescending(x => x.Category);
                        ViewBag.currentSort = "categoryDesc";
                        break;
                    default:
                        incomes = incomes.OrderBy(x => x.IncomeID);
                        break;
                }

                return View(incomes.ToPagedList(pageNumber, pageSize));
            }
        }

        public ViewResult Edit(int incomeID)
        {
            Income income = repository.Incomes.FirstOrDefault(i => i.IncomeID == incomeID);

            ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Income"), "Name", "Name");

            return View(income);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Income income)
        {
            if (ModelState.IsValid)
            {
                repository.Save(income);
                TempData["message"] = string.Format("Zaktualizowano {0}", income.Description);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Income"), "Name", "Name");
                return View(income);
            }
        }

        public ViewResult Create()
        {
            ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Income"), "Name", "Name");
            return View("Create", new Income());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Income income)
        {
            if (ModelState.IsValid)
            {
                repository.Save(income);
                TempData["message"] = string.Format("Utworzono {0}", income.Description);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Income"), "Name", "Name");
                return View(income);
            }
        }

        [HttpPost]
        public ActionResult Delete(int incomeID)
        {
            Income deletedIncome = repository.Delete(incomeID);

            if (deletedIncome != null)
            {
                TempData["message"] = string.Format("Usunięto {0} z dnia {1}", deletedIncome.Description, deletedIncome.Date.ToString("d"));
            }

            return RedirectToAction("Index");
        }
    }
}