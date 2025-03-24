using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Spendo_Backend.Controllers;
using Spendo_Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Spendo_Tests
{
    public class BudgetControllerTests
    {
        private readonly Mock<SpendoContext> _mockContext;
        private readonly BudgetsController _controller;

        public BudgetControllerTests()
        {
            // Maak de mock voor SpendoContext
            _mockContext = new Mock<SpendoContext>();
            _controller = new BudgetsController(_mockContext.Object);
        }

        // Helperfunctie om DbSet te mocken
        private Mock<DbSet<T>> GetMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            // Stel de DbSet in om IQueryable te simuleren
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            // Simuleer FirstOrDefault zonder de async extensie
            mockSet.Setup(m => m.FirstOrDefault(It.IsAny<Expression<Func<T, bool>>>()))
                   .Returns((Expression<Func<T, bool>> predicate) => queryable.FirstOrDefault(predicate));

            return mockSet;
        }


        [Fact]
        public async Task GetTotalBudget_ReturnsOk_WhenBudgetExists()
        {
            var categoryId = 1;
            var budget = new Budget { CategoryId = categoryId, Amount = 1000 };

            // Mock de Budgets DbSet
            var budgets = new List<Budget> { budget };
            _mockContext.Setup(c => c.Budgets).Returns(GetMockDbSet(budgets).Object);

            var result = await _controller.GetTotalBudget(categoryId);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(1000m, actionResult.Value);
        }

        [Fact]
        public async Task GetTotalBudget_ReturnsNotFound_WhenBudgetDoesNotExist()
        {
            var categoryId = 1;

            // Mock de Budgets DbSet met lege lijst
            var budgets = new List<Budget>();
            _mockContext.Setup(c => c.Budgets).Returns(GetMockDbSet(budgets).Object);

            var result = await _controller.GetTotalBudget(categoryId);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetRemainingBudget_ReturnsOk_WhenBudgetExists()
        {
            var categoryId = 1;
            var budget = new Budget { CategoryId = categoryId, Amount = 1000 };
            var transaction = new Transaction { Type = "Expense", Amount = 200 };

            // Mock de Budgets en Transactions DbSet
            var budgets = new List<Budget> { budget };
            var transactions = new List<Transaction> { transaction };

            _mockContext.Setup(c => c.Budgets).Returns(GetMockDbSet(budgets).Object);
            _mockContext.Setup(c => c.Transactions).Returns(GetMockDbSet(transactions).Object);

            var result = await _controller.GetRemainingBudget(categoryId);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(800m, actionResult.Value);
        }
    }
}
