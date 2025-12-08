using PersonalFinance.Data;
using PersonalFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllTransactionsAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllIncomeTransactionsAsync()
        {
            return await _transactionRepository.GetAllIncomeTransactionsAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllExpenseTransactionsAsync()
        {
            return await _transactionRepository.GetAllExpenseTransactionsAsync();
        }

        public async Task<FinancialTransaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }

        public async Task AddTransactionAsync(FinancialTransaction transaction)
        {
            await _transactionRepository.AddAsync(transaction);
        }

        public async Task EditTransactionAsync(FinancialTransaction transaction)
        {
            await _transactionRepository.EditAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteAsync(id);
        }
    }
}
