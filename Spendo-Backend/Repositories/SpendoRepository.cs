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
        public async Task<Decimal> GetTotalTransactionsDecimalMonth()
        {
            return await _context.Transactions
               .Where(t => t.Type == "Expense")
               .SumAsync(t => t.Amount);
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
