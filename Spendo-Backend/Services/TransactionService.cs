using Spendo_Backend.Models;
using Spendo_Backend.Repositories;

namespace Spendo_Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ISpendoRepository _spendoRepository;
        public TransactionService(ISpendoRepository spendoRepository)
        {
            _spendoRepository = spendoRepository;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _spendoRepository.GetAllTransactionsAsync();
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            var totalExpenses = await _spendoRepository.GetTotalTransactionsDecimalMonth();
         
            return await _spendoRepository.CreateTransaction(transaction);
        }
    }
}
