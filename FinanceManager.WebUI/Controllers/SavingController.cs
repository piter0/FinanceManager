using System;
using System.Linq;
using System.Web.Mvc;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;
using PagedList;

namespace FinanceManager.WebUI.Controllers
{
    public class SavingController : Controller
    {
        private ISavingRepository repository;

        private ICategoryRepository categoryRepository;

        public SavingController(ISavingRepository savingRepository, ICategoryRepository categoryRepository)
        {
            repository = savingRepository;
            this.categoryRepository = categoryRepository;
        }

        public ActionResult Index(string date, string sortBy, string currentSort, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewBag.dateParam = sortBy == "date" ? "dateDesc" : "date";
            ViewBag.sumParam = sortBy == "sum" ? "sumDesc" : "sum";
            ViewBag.categoryParam = sortBy == "category" ? "categoryDesc" : "category";
            ViewBag.controllerName = "Saving"; //information for Navigation controller

            if (date == null)
            {
                date = DateTime.Now.ToString("MM-yyyy");
                ViewBag.selectedDate = date;

                var savings = repository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

                return View(savings.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var savings = repository.Savings.Where(x => x.Date.ToString("MM-yyyy").Equals(date));

                ViewBag.selectedDate = date;

                switch (sortBy)
                { 
                    case "date":
                        savings = savings.OrderBy(x => x.Date);
                        ViewBag.currentSort = "date";
                        break;
                    case "dateDesc":
                        savings = savings.OrderByDescending(x => x.Date);
                        ViewBag.currentSort = "dateDesc";
                        break;
                    case "sum":
                        savings = savings.OrderBy(x => x.Price);
                        ViewBag.currentSort = "sum";
                        break;
                    case "sumDesc":
                        savings = savings.OrderByDescending(x => x.Price);
                        ViewBag.currentSort = "sumDesc";
                        break;
                    case "category":
                        savings = savings.OrderBy(x => x.Category);
                        ViewBag.currentSort = "category";
                        break;
                    case "categoryDesc":
                        savings = savings.OrderByDescending(x => x.Category);
                        ViewBag.currentSort = "categoryDesc";
                        break;
                    default:
                        savings = savings.OrderBy(x => x.SavingID);
                        break;
                }

                return View(savings.ToPagedList(pageNumber, pageSize));
            }
        }

        public ViewResult Edit(int savingID)
        {
            Saving saving = repository.Savings.FirstOrDefault(i => i.SavingID == savingID);

            ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Saving"), "Name", "Name");

            return View(saving);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Saving saving)
        {
            if (ModelState.IsValid)
            {
                repository.Save(saving);
                TempData["message"] = string.Format("Zaktualizowano {0}", saving.Description);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Saving"), "Name", "Name");
                return View(saving);
            }
        }

        public ViewResult Create()
        {
            ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Saving"), "Name", "Name");
            return View("Create", new Saving());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Saving saving)
        {
            if (ModelState.IsValid)
            {
                repository.Save(saving);
                TempData["message"] = string.Format("Utworzono {0}", saving.Description);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Category = new SelectList(categoryRepository.Categories.Where(x => x.Type == "Saving"), "Name", "Name");
                return View(saving);
            }
        }

        [HttpPost]
        public ActionResult Delete(int savingID)
        {
            Saving deletedSaving = repository.Delete(savingID);

            if (deletedSaving != null)
            {
                TempData["message"] = string.Format("Usunięto {0} z dnia {1}", deletedSaving.Description, deletedSaving.Date.ToString("d"));
            }

            return RedirectToAction("Index");
        }
    }
}