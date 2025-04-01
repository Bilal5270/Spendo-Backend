using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Spendo_Backend.Services;
using Spendo_Backend.Models;
using Spendo_Backend.Repositories;

namespace Spendo_Tests
{
    public class BudgetServiceTests
    {
        private readonly Mock<ISpendoRepository> _mockRepository;
        private readonly BudgetService _budgetService;

        public BudgetServiceTests()
        {
            _mockRepository = new Mock<ISpendoRepository>();
            _budgetService = new BudgetService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetTotalBudget_ShouldReturnBudget_WhenBudgetExists()
        {
            // Arrange
            int categoryId = 1;
            var budget = new Budget { CategoryId = categoryId, Amount = 1000 };

            _mockRepository.Setup(r => r.GetTotalBudgetMonth(categoryId))
                           .ReturnsAsync(budget);

            // Act
            var result = await _budgetService.GetTotalBudget(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000m, result.Amount);
        }

        [Fact]
        public async Task GetTotalBudget_ShouldThrowException_WhenBudgetNotFound()
        {
            // Arrange
            int categoryId = 1;
            _mockRepository.Setup(r => r.GetTotalBudgetMonth(categoryId))
                           .ReturnsAsync((Budget)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _budgetService.GetTotalBudget(categoryId));
            Assert.Equal("Budget voor deze categorie niet gevonden.", exception.Message);
        }

        [Fact]
        public async Task GetRemainingBudget_ShouldReturnCorrectAmount()
        {
            // Arrange
            int categoryId = 1;
            var budget = new Budget { CategoryId = categoryId, Amount = 1000 };
            decimal totalExpenses = 250m;

            _mockRepository.Setup(r => r.GetTotalBudgetMonth(categoryId))
                           .ReturnsAsync(budget);
            _mockRepository.Setup(r => r.GetTotalTransactionsDecimalMonth())
                           .ReturnsAsync(totalExpenses);

            // Act
            var result = await _budgetService.GetRemainingBudget(categoryId);

            // Assert
            Assert.Equal(750m, result); // 1000 - 250
        }

        [Fact]
        public async Task GetRemainingBudget_ShouldThrowException_WhenBudgetNotFound()
        {
            // Arrange
            int categoryId = 1;
            _mockRepository.Setup(r => r.GetTotalBudgetMonth(categoryId))
                           .ReturnsAsync((Budget)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _budgetService.GetRemainingBudget(categoryId));
            Assert.Equal("Budget voor deze categorie niet gevonden.", exception.Message);
        }
    }
}
