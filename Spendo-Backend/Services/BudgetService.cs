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

        public async Task<Budget?> GetTotalBudget(int categoryId)
        {
            var generalBudget = await _spendoRepository.GetTotalBudgetMonth(categoryId);
            // Geen exception — gewoon null teruggeven
            return generalBudget;
        }

        public async Task<Budget> CreateBudget(Budget budget)
        {
            var existingBudget = await _spendoRepository.GetTotalBudgetMonth(6);

            if (existingBudget != null)
            {
                existingBudget.Amount = budget.Amount;
                await _spendoRepository.SaveChanges();
                return existingBudget;
            }

            var budgetMonth = new Budget
            {
                CategoryId = 6,
                Amount = budget.Amount,
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month
            };

            return await _spendoRepository.CreateBudget(budgetMonth);
        }

        public async Task<decimal?> GetRemainingBudget(int categoryId)
        {
            var generalBudget = await _spendoRepository.GetTotalBudgetMonth(categoryId);

            if (generalBudget == null)
            {
                // Geen crash — geef null terug
                return null;
            }

            var totalExpenses = await _spendoRepository.GetTotalTransactionsDecimalMonth();
            var remainingBudget = generalBudget.Amount - totalExpenses;

            return remainingBudget;
        }
    }
}
