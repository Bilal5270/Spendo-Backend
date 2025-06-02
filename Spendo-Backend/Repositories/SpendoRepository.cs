using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;
using Spendo_Backend.Models.DTO;

namespace Spendo_Backend.Repositories
{
    public class SpendoRepository :ISpendoRepository
    {
        private readonly SpendoContext _context;
        public SpendoRepository(SpendoContext context)
        {
            _context = context;
        }

        public async Task<Budget> GetTotalBudgetMonth(int categoryId)
        {
            return await _context.Budgets
                .Where(b => b.CategoryId == categoryId && b.Year == DateTime.Now.Year && b.Month == DateTime.Now.Month)
                .FirstOrDefaultAsync();
        }

        public async Task<Budget> CreateBudget(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
            return budget;
        }
        public async Task<decimal> GetTotalTransactionsDecimalMonth()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayNextMonth = firstDayOfMonth.AddMonths(1);

            return await _context.Transactions
                .Where(t => t.Type == "Expense"
                    && t.TransactionDate >= firstDayOfMonth
                    && t.TransactionDate < firstDayNextMonth)
                .SumAsync(t => t.Amount);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                //.Include(t => t.Category) // optioneel, als je categoriegegevens mee wil laden
                .ToListAsync();
        }
        public async Task<List<RecurringTransaction>> GetAllRecurringTransactionsAsync()
        {
            return await _context.RecurringTransactions
                .Include(rt => rt.Category)
                .ToListAsync();
        }




        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            if (transaction.TransactionDate.HasValue)
            {
                transaction.TransactionDate = DateTime.SpecifyKind(transaction.TransactionDate.Value, DateTimeKind.Unspecified);
            }

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
