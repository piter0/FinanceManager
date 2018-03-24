using System.Collections.Generic;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Concrete
{
    public class EFIncomeRepository : IIncomeRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Income> Incomes
        {
            get { return context.Incomes; }
        }

        public void Save(Income income)
        {
            if (income.IncomeID == 0)
            {
                context.Incomes.Add(income);
            }
            else
            {
                Income dbEntry = context.Incomes.Find(income.IncomeID);
                if (dbEntry != null)
                {
                    dbEntry.Description = income.Description;
                    dbEntry.Category = income.Category;
                    dbEntry.Price = income.Price;
                    dbEntry.Date = income.Date;
                }
            }

            context.SaveChanges();
        }

        public Income Delete(int incomeID)
        {
            Income dbEntry = context.Incomes.Find(incomeID);
            if (dbEntry != null)
            {
                context.Incomes.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
