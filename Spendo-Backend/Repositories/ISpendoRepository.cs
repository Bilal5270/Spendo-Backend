﻿using Spendo_Backend.Models;

namespace Spendo_Backend.Repositories
{
    public interface ISpendoRepository
    {
        public Task<Budget> CreateBudget(Budget budget);
        public Task<Budget> GetTotalBudgetMonth(int categoryId);
        public Task<Decimal> GetTotalTransactionsDecimalMonth();
        public Task SaveChanges();
    }
}
