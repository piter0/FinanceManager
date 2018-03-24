using System.Collections.Generic;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Abstract
{
    public interface ISavingRepository
    {
        IEnumerable<Saving> Savings { get; }

        void Save(Saving saving);

        Saving Delete(int savingID);
    }
}
