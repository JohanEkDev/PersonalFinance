using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.DTOs
{
    public class MonthlyForecastResult
    {
        public int TotalIncome { get; set; }
        public int TotalExpense { get; set; }

        public List<ForecastItem> Incomes { get; set; } = new();
        public List<ForecastItem> Expenses { get; set; } = new();
    }
}
