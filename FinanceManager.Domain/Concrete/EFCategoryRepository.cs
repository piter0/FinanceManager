using System.Collections.Generic;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Category> Categories
        {
            get { return context.Categories;  }
        }

        public void Save(Category category)
        {
            if (category.CategoryID == 0)
            {
                context.Categories.Add(category);
            }
            else
            {
                Category dbEntry = context.Categories.Find(category.CategoryID);
                if (dbEntry != null)
                {
                    dbEntry.Type = category.Type;
                    dbEntry.Name = category.Name;
                }
            }

            context.SaveChanges();
        }

        public Category Delete(int categoryID)
        {
            Category dbEntry = context.Categories.Find(categoryID);
            if (dbEntry != null)
            {
                context.Categories.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
