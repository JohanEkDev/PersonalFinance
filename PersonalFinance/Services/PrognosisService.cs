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

        public async Task<MonthlyResult> GetMonthlyPrognosisAsync(DateTime month)
        {
            if (month <= DateTime.Today)
                throw new ArgumentException("Prognosis must be for a future month.");

            var allTransactions = await _transactionRepository.GetAllTransactionsIncludeCategoriesAsync();

            var applicable = allTransactions
                .Where(t => IsApplicableForMonthPrognosis(t, month))
                .ToList();

            var income = applicable.Where(t => t.Type == TypeOfTransaction.Income).ToList();
            var expense = applicable.Where(t => t.Type == TypeOfTransaction.Expense).ToList();

            return new MonthlyResult
            {
                IncomeTransactions = income,
                ExpenseTransactions = expense,
                TotalIncome = income.Sum(t => t.Amount),
                TotalExpense = expense.Sum(t => t.Amount)
            };
        }

        public async Task<YearlyResult> GetYearlyPrognosisAsync(int year)
        {
            var allTransactions =
                await _transactionRepository.GetAllTransactionsIncludeCategoriesAsync();

            int totalIncome = 0;
            int totalExpense = 0;

            foreach (var t in allTransactions)
            {
                NormalizeTransaction(t);

                int yearlyAmount = t.Frequency switch
                {
                    FrequencyOfTransaction.OneTime =>
                        t.StartDate.Year == year ? t.Amount : 0,

                    FrequencyOfTransaction.Monthly =>
                        CalculateMonthlyPrognosisContribution(t, year),

                    FrequencyOfTransaction.Yearly =>
                        CalculateYearlyContribution(t, year),

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

        private bool IsApplicableForMonthPrognosis(FinancialTransaction transaction, DateTime month)
        {
            var periodStart = new DateTime(month.Year, month.Month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);

            if (transaction.Frequency == FrequencyOfTransaction.OneTime)
            {
                return transaction.StartDate.Year == month.Year &&
                       transaction.StartDate.Month == month.Month;
            }

            // Monthly / Yearly
            if (transaction.StartDate > periodEnd)
                return false;

            if (transaction.EndDate.HasValue && transaction.EndDate.Value < periodStart)
                return false;

            if (transaction.Frequency == FrequencyOfTransaction.Yearly)
                return transaction.StartDate.Month == month.Month;

            return true; // Monthly
        }

        private int CalculateMonthlyPrognosisContribution(FinancialTransaction transaction, int year)
        {
            if (transaction.StartDate.Year > year)
                return 0;

            if (transaction.EndDate.HasValue && transaction.EndDate.Value.Year < year)
                return 0;

            int startMonth = transaction.StartDate.Year == year
                ? transaction.StartDate.Month
                : 1;

            int endMonth = transaction.EndDate.HasValue && transaction.EndDate.Value.Year == year
                ? transaction.EndDate.Value.Month
                : 12;

            if (endMonth < startMonth)
                return 0;

            return (endMonth - startMonth + 1) * transaction.Amount;
        }

        private int CalculateYearlyContribution(FinancialTransaction transaction, int year)
        {
            if (transaction.StartDate.Year > year)
                return 0;

            if (transaction.EndDate.HasValue && transaction.EndDate.Value.Year < year)
                return 0;

            return transaction.Amount;
        }

        private void NormalizeTransaction(FinancialTransaction transaction)
        {
            if (transaction.Frequency == FrequencyOfTransaction.OneTime)
                transaction.EndDate = null;
        }

    }
}
