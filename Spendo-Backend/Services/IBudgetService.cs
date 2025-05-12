using Spendo_Backend.Models;

namespace Spendo_Backend.Services
{
    public interface IBudgetService
    {
        public Task<Budget> CreateBudget(Budget budget);
        public Task<decimal> GetRemainingBudget(int categoryId);
        public Task<Budget> GetTotalBudget(int categoryId);
        //public Task<Budget> UpdateBudgetAsync(Budget budget);
    }
}
