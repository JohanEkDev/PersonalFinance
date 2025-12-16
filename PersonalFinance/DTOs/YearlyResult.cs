using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.DTOs
{
    public class YearlyResult
    {
        public int Year { get; set; }

        public int TotalIncome { get; set; }
        public int TotalExpense { get; set; }

        public int Net => TotalIncome - TotalExpense;
    }
}
