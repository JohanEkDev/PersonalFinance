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
        Task<Transaction?> GetTransactionByIdAsync();
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Transaction>> GetAllIncomeTransactionsAsync();
        Task<IEnumerable<Transaction>> GetAllExpenseTransactionsAsync();
        void SaveAsync();
        void EditAsync();
        void DeleteAsync();
    }
}
