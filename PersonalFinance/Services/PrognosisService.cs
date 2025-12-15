using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;
using PersonalFinance.DTOs;
using PersonalFinance.Enums;
using PersonalFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Services
{
    public class PrognosisService : IPrognosisService
    {
        private readonly ITransactionRepository _transactionRepository;

        public PrognosisService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<MonthlyForecastResult> GetMonthlyForecastAsync(DateTime selectedMonth)
        {
            var transactions = await _transactionRepository.GetAllTransactionsIncludeCategoriesAsync();

            var applicable = transactions
                .Where(t => AppliesToMonth(t, selectedMonth))
                .ToList();

            var incomes = applicable
                .Where(t => t.Type == TypeOfTransaction.Income)
                .Select(t => ToForecastItem(t))
                .ToList();

            var expenses = applicable
                .Where(t => t.Type == TypeOfTransaction.Expense)
                .Select(t => ToForecastItem(t))
                .ToList();

            return new MonthlyForecastResult
            {
                TotalIncome = incomes.Sum(i => i.Amount),
                TotalExpense = expenses.Sum(e => e.Amount),
                Incomes = incomes,
                Expenses = expenses
            };
        }

        private static ForecastItem ToForecastItem(FinancialTransaction transaction)
        {
            return new ForecastItem
            {
                Category = transaction.Category.Name,
                Amount = transaction.Amount,
                StartDate = transaction.StartDate,
                Frequency = transaction.Frequency
            };
        }

        private static bool AppliesToMonth(FinancialTransaction transaction, DateTime selectedMonth)
        {
            var target = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);

            return transaction.Frequency switch
            {
                FrequencyOfTransaction.OneTime =>
                    transaction.StartDate.Year == target.Year &&
                    transaction.StartDate.Month == target.Month,

                FrequencyOfTransaction.Monthly =>
                    transaction.StartDate <= target &&
                    (transaction.EndDate == null || transaction.EndDate >= target),

                FrequencyOfTransaction.Yearly =>
                    transaction.StartDate.Month == target.Month,

                _ => false
            };
        }
    }
}
