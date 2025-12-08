using Microsoft.EntityFrameworkCore;
using PersonalFinance.Enums;
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

        public async Task<FinancialTransaction?> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllTransactionsAsync()
        {
           return await _context.Transactions.ToListAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllIncomeTransactionsAsync()
        {
            return await _context.Transactions.Where(t => t.Type == TypeOfTransaction.Income).ToListAsync();
        }

        public async Task<IEnumerable<FinancialTransaction>> GetAllExpenseTransactionsAsync()
        {
            return await _context.Transactions.Where(t => t.Type == TypeOfTransaction.Expense).ToListAsync();
        }

        public async Task AddAsync(FinancialTransaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(FinancialTransaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
