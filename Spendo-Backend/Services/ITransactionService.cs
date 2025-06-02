using Spendo_Backend.Models;
using Spendo_Backend.Models.DTO;

namespace Spendo_Backend.Services
{
    public interface ITransactionService
    {
        public Task<Transaction> CreateTransaction(Transaction transaction);
        public Task<List<RecurringTransactionDTO>> GetAllConvertedRecurringTransactionsAsync();
        public Task<List<Transaction>> GetAllTransactions();
    }
}
