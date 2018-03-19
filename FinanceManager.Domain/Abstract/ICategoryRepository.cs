using System.Collections.Generic;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Abstract
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }

        void Save(Category category);

        Category Delete(int categoryID);
    }
}
