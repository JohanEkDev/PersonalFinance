using PersonalFinance.Data;
using PersonalFinance.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly ITransactionRepository _transactionRepo;

        public SummaryService(ITransactionRepository transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public async Task<int> GetTotalIncomeAsync()
        {
            var transactions = await _transactionRepo.GetAllIncomeTransactionsAsync();
            return transactions
                .Sum(t => t.Amount);
        }

        public async Task<int> GetTotalExpenseAsync()
        {
            var transactions = await _transactionRepo.GetAllExpenseTransactionsAsync();
            return transactions
                .Sum(t => t.Amount);
        }
    }
}
