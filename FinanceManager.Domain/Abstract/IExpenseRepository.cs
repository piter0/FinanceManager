using System.Collections.Generic;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Abstract
{
    public interface IExpenseRepository
    {
        IEnumerable<Expense> Expenses { get; }

        void Save(Expense expense);

        Expense Delete(int expenseID);
    }
}
