using Spendo_Backend.Models;

namespace Spendo_Backend.Services
{
    public interface ITransactionService
    {
        public Task<Transaction> CreateTransaction(Transaction transaction);
        public Task<List<Transaction>> GetAllTransactions();
    }
}
