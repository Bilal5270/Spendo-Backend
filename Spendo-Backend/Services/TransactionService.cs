using Spendo_Backend.Models;
using Spendo_Backend.Models.DTO;
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

        public async Task<Transaction?> CreateTransaction(Transaction transaction)
        {
            if (transaction == null)
                return null; 

            try
            {
              
                return await _spendoRepository.CreateTransaction(transaction);
            }
            catch (Exception ex)
            {
          
                return null; 
            }
        }

        public async Task<List<RecurringTransactionDTO>> GetAllConvertedRecurringTransactionsAsync()
        {
            var transactions = await _spendoRepository.GetAllRecurringTransactionsAsync();

            if (transactions == null || !transactions.Any())
                return new List<RecurringTransactionDTO>();

            return transactions.Select(rt => new RecurringTransactionDTO
            {
                RecurringId = rt.RecurringId,
                CategoryName = rt.Category?.Name,
                Description = rt.Description,
                Type = rt.Type,
                OriginalAmount = rt.Amount,
                RecurrenceInterval = rt.RecurrenceInterval,
                WeeklyAmount = ConvertToWeekly(rt.Amount, rt.RecurrenceInterval),
                MonthlyAmount = ConvertToMonthly(rt.Amount, rt.RecurrenceInterval),
                YearlyAmount = ConvertToYearly(rt.Amount, rt.RecurrenceInterval)
            }).ToList();
        }

        private decimal ConvertToWeekly(decimal amount, string interval)
        {
            return interval.ToLower() switch
            {
                "weekly" => amount,
                "monthly" => amount / 4.33m,
                "yearly" => amount / 52m,
                _ => amount
            };
        }

        private decimal ConvertToMonthly(decimal amount, string interval)
        {
            return interval.ToLower() switch
            {
                "weekly" => amount * 4.33m,
                "monthly" => amount,
                "yearly" => amount / 12m,
                _ => amount
            };
        }

        private decimal ConvertToYearly(decimal amount, string interval)
        {
            return interval.ToLower() switch
            {
                "weekly" => amount * 52m,
                "monthly" => amount * 12m,
                "yearly" => amount,
                _ => amount
            };
        }
    }
}
