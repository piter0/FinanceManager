using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Domain.Entities
{
    public class Summary
    {
        public decimal ExpensesSum { get; set; }
        public decimal IncomesSum { get; set; }
        public decimal SavingsSum { get; set; }
        public double ExpensesToIncomes { get; set; }
        public double Incomes { get; set; }
        public double SavingsToIncomes { get; set; }
        public int ExpensesRecords { get; set; }
        public int IncomesRecords { get; set; }
        public int SavingsRecords { get; set; }

        public string Date { get; set; }
    }
}