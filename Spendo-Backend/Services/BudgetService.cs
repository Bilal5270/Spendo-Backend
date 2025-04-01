using Spendo_Backend.Models;
using Spendo_Backend.Repositories;

namespace Spendo_Backend.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ISpendoRepository _spendoRepository;
        public BudgetService(ISpendoRepository spendoRepository)
        {
            _spendoRepository = spendoRepository;
        }

        public async Task<Budget> GetTotalBudget(int categoryId)
        {
            var generalBudget = await _spendoRepository.GetTotalBudgetMonth(categoryId);
            if (generalBudget == null)
            {
                throw new Exception("Budget voor deze categorie niet gevonden.");
            }
            return generalBudget;
        }

        public async Task<decimal> GetRemainingBudget(int categoryId)
        {
            var generalBudget = await _spendoRepository.GetTotalBudgetMonth(categoryId);
            if (generalBudget == null)
            {
                throw new Exception("Budget voor deze categorie niet gevonden.");
            }
            var totalExpenses = await _spendoRepository.GetTotalTransactionsDecimalMonth();
            var remainingBudget = generalBudget.Amount - totalExpenses;
            return remainingBudget;
        }
    }
}
