using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;

namespace FinanceManager.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            repository = categoryRepository;
        }

        public ActionResult Index(string type)
        {
            if (type == "Expense")
            {
                var categories = repository.Categories.Where(x => x.Type == "Expense");
                ViewBag.categoryType = "Expense";

                return View(categories);
            }
            else if (type == "Income")
            {
                var categories = repository.Categories.Where(x => x.Type == "Income");
                ViewBag.categoryType = "Income";

                return View(categories);
            }
            else
            {
                var categories = repository.Categories.Where(x => x.Type == "Saving");
                ViewBag.categoryType = "Saving";

                return View(categories);
            }
        }

        public ViewResult Edit(int categoryID)
        {
            Category category = repository.Categories.FirstOrDefault(i => i.CategoryID == categoryID);

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (repository.Categories.Any(x => x.Name.Equals(category.Name, StringComparison.CurrentCultureIgnoreCase) &&  x.Type.Equals(category.Type)))                    
                {
                    TempData["error"] = string.Format("Kategoria {0} już istnieje", category.Name);
                    return RedirectToAction("Index", new { type = category.Type });
                }
                else
                {
                    repository.Save(category);
                    TempData["message"] = string.Format("Zaktualizowano kategorię {0}", category.Name);
                    return RedirectToAction("Index", new { type = category.Type });
                }
            }
            else
            {
                return View(category);
            }
        }

        public ViewResult Create(string type)
        {
            return View("Create", new Category() { Type = type });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (repository.Categories.Any(x => x.Name.Equals(category.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    TempData["error"] = string.Format("Kategoria {0} już istnieje", category.Name);
                    return RedirectToAction("Index", new { type = category.Type });
                }
                else
                {
                    repository.Save(category);
                    TempData["message"] = string.Format("Utworzono kategorię {0}", category.Name);

                    return RedirectToAction("Index", new { type = category.Type });
                }
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public ActionResult Delete(int categoryID)
        {
            Category deletedCategory = repository.Delete(categoryID);

            if (deletedCategory != null)
            {
                TempData["message"] = string.Format("Usunięto kategorię {0}", deletedCategory.Name);
            }

            return RedirectToAction("Index", new { type = deletedCategory.Type });
        }
    }
}