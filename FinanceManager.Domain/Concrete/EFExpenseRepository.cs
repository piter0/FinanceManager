using System.Collections.Generic;
using FinanceManager.Domain.Abstract;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Concrete
{
    public class EFExpenseRepository : IExpenseRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Expense> Expenses
        {
            get { return context.Expenses; }
        }

        public void Save(Expense expense)
        {
            if (expense.ExpenseID == 0)
            {
                context.Expenses.Add(expense);
            }
            else
            {
                Expense dbEntry = context.Expenses.Find(expense.ExpenseID);
                if (dbEntry != null)
                {
                    dbEntry.Description = expense.Description;
                    dbEntry.Category = expense.Category;
                    dbEntry.Price = expense.Price;
                    dbEntry.Date = expense.Date;
                }
            }

            context.SaveChanges();
        }

        public Expense Delete(int itemID)
        {
            Expense dbEntry = context.Expenses.Find(itemID);
            if (dbEntry != null)
            {
                context.Expenses.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
