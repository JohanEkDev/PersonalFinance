using PersonalFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Data
{
    public interface ITransactionRepository
    {
        Task<FinancialTransaction?> GetTransactionByIdAsync(int id);
        Task<IEnumerable<FinancialTransaction>> GetAllTransactionsAsync();
        Task<IEnumerable<FinancialTransaction>> GetAllTransactionsIncludeCategoriesAsync();
        Task<IEnumerable<FinancialTransaction>> GetAllIncomeTransactionsAsync();
        Task<IEnumerable<FinancialTransaction>> GetAllExpenseTransactionsAsync();
        Task AddAsync(FinancialTransaction transaction);
        Task EditAsync(FinancialTransaction transaction);
        Task DeleteAsync(int id);
    }
}
