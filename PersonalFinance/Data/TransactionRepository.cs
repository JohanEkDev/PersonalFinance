using Microsoft.EntityFrameworkCore;
using PersonalFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Transaction>> GetAllExpenseTransactionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAllIncomeTransactionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Transaction?> GetTransactionByIdAsync()
        {
            throw new NotImplementedException();
        }

        public void SaveAsync()
        {
            _context.SaveChangesAsync();
        }

        public void EditAsync()
        {
            _context.SaveChangesAsync();
        }

        public void DeleteAsync(int id)
        {


            _context.Remove(id);
        }
    }
}
