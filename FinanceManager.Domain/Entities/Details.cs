using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Domain.Entities
{
    public class Details
    {
        public string Date { get; set; }
        public List<Tuple<string, decimal, int, double>> DetailedList { get; set; }
        public decimal CategorySum { get; set; }
        public string Coniugation { get; set; }
    }
}
