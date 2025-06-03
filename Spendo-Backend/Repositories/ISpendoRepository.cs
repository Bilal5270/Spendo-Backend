using Spendo_Backend.Models;

namespace Spendo_Backend.Repositories
{
    public interface ISpendoRepository
    {
        public Task<Budget> CreateBudget(Budget budget);
        public Task<RecurringTransaction> CreateRecurringTransaction(RecurringTransaction recurringTransaction);
        public Task<Transaction> CreateTransaction(Transaction transaction);
        public Task<List<RecurringTransaction>> GetAllRecurringTransactionsAsync();
        public Task<List<Transaction>> GetAllTransactionsAsync();
        public Task<Budget> GetTotalBudgetMonth(int categoryId);
        public Task<Decimal> GetTotalTransactionsDecimalMonth();
        public Task SaveChanges();
    }
}
