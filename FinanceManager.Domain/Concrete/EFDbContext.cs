using FinanceManager.Domain.Entities;
using System.Data.Entity;

namespace FinanceManager.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Saving> Savings { get; set; }
    }
}
