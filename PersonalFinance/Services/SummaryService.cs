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
    public class SummaryService : ISummaryService
    {
        private readonly ITransactionRepository _transactionRepository;

        public SummaryService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<MonthlyResult> GetMonthlySummaryAsync(DateTime month)
        {
            DateTime monthStart = new DateTime(month.Year, month.Month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddTicks(-1);

            var allTransactions =
                await _transactionRepository.GetAllTransactionsIncludeCategoriesAsync();

            var applicable = allTransactions
                .Where(t => IsApplicableForMonthSummary(t, monthStart, monthEnd))
                .ToList();

            var income = applicable
                .Where(t => t.Type == TypeOfTransaction.Income)
                .ToList();

            var expense = applicable
                .Where(t => t.Type == TypeOfTransaction.Expense)
                .ToList();

            return new MonthlyResult
            {
                IncomeTransactions = income,
                ExpenseTransactions = expense,
                TotalIncome = income.Sum(t => t.Amount),
                TotalExpense = expense.Sum(t => t.Amount)
            };
        }

        public async Task<YearlyResult> GetYearlySummaryAsync(int year)
        {
            var allTransactions =
                await _transactionRepository.GetAllTransactionsIncludeCategoriesAsync();

            int totalIncome = 0;
            int totalExpense = 0;

            foreach (var t in allTransactions)
            {
                int yearlyAmount = t.Frequency switch
                {
                    FrequencyOfTransaction.OneTime =>
                        t.StartDate.Year == year ? t.Amount : 0,

                    FrequencyOfTransaction.Monthly =>
                        CalculateMonthlySummaryContribution(t, year),

                    FrequencyOfTransaction.Yearly =>
                        CalculateYearlySummaryContribution(t, year),

                    _ => 0
                };

                if (t.Type == TypeOfTransaction.Income)
                    totalIncome += yearlyAmount;
                else
                    totalExpense += yearlyAmount;
            }

            return new YearlyResult
            {
                Year = year,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense
            };
        }

        private bool IsApplicableForMonthSummary(FinancialTransaction transaction, DateTime monthStart, DateTime monthEnd)
        {
            if (transaction.Frequency == FrequencyOfTransaction.OneTime)
            {
                return transaction.StartDate >= monthStart &&
                       transaction.StartDate <= monthEnd;
            }

            // Monthly / Yearly
            if (transaction.StartDate > monthEnd)
                return false;

            if (transaction.EndDate.HasValue && transaction.EndDate.Value < monthStart)
                return false;

            if (transaction.Frequency == FrequencyOfTransaction.Yearly)
                return transaction.StartDate.Month == monthStart.Month;

            return true; // Monthly
        }

        private int CalculateMonthlySummaryContribution(FinancialTransaction transaction, int year)
        {
            if (transaction.StartDate.Year > year)
                return 0;

            if (transaction.EndDate.HasValue && transaction.EndDate.Value.Year < year)
                return 0;

            int startMonth =
                transaction.StartDate.Year == year ? transaction.StartDate.Month : 1;

            int endMonth =
                transaction.EndDate.HasValue && transaction.EndDate.Value.Year == year
                    ? transaction.EndDate.Value.Month
                    : 12;

            if (endMonth < startMonth)
                return 0;

            return (endMonth - startMonth + 1) * transaction.Amount;
        }

        private int CalculateYearlySummaryContribution(FinancialTransaction transaction, int year)
        {
            if (transaction.StartDate.Year > year)
                return 0;

            if (transaction.EndDate.HasValue && transaction.EndDate.Value.Year < year)
                return 0;

            return transaction.Amount;
        }
    }

}
