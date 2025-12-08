using PersonalFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<FinancialTransaction>> GetAllTransactionsAsync();
        Task<IEnumerable<FinancialTransaction>> GetAllIncomeTransactionsAsync();
        Task<IEnumerable<FinancialTransaction>> GetAllExpenseTransactionsAsync();
        Task<FinancialTransaction?> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(FinancialTransaction transaction);
        Task EditTransactionAsync(FinancialTransaction transaction);
        Task DeleteTransactionAsync(int id);

    }
}
