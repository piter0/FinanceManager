using System.Collections.Generic;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Concrete
{
    public class EFSavingRepository : ISavingRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Saving> Savings
        {
            get { return context.Savings; }
        }

        public void Save(Saving saving)
        {
            if (saving.SavingID == 0)
            {
                context.Savings.Add(saving);
            }
            else
            {
                Saving dbEntry = context.Savings.Find(saving.SavingID);
                if (dbEntry != null)
                {
                    dbEntry.Description = saving.Description;
                    dbEntry.Category = saving.Category;
                    dbEntry.Price = saving.Price;
                    dbEntry.Date = saving.Date;
                }
            }

            context.SaveChanges();
        }

        public Saving Delete(int savingID)
        {
            Saving dbEntry = context.Savings.Find(savingID);
            if (dbEntry != null)
            {
                context.Savings.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
