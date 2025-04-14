using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;

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
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
