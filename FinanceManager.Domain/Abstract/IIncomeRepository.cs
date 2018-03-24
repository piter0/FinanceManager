using System.Collections.Generic;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Abstract
{
    public interface IIncomeRepository
    {
        IEnumerable<Income> Incomes { get; }

        void Save(Income income);

        Income Delete(int incomeID);
    }
}
