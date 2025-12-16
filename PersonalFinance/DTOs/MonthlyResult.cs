using PersonalFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.DTOs
{
    public class MonthlyResult
    {
        public IReadOnlyList<FinancialTransaction> IncomeTransactions { get; init; } = [];
        public IReadOnlyList<FinancialTransaction> ExpenseTransactions { get; init; } = [];

        public int TotalIncome { get; init; }
        public int TotalExpense { get; init; }
    }
}
