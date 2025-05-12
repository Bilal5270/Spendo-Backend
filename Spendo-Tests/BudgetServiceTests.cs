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
        [Fact]
        public async Task CreateBudget_ShouldUpdateExistingBudget_WhenItExists()
        {
            // Arrange
            var inputBudget = new Budget { Amount = 500 };
            var existingBudget = new Budget { CategoryId = 6, Amount = 100 };

            _mockRepository.Setup(r => r.GetTotalBudgetMonth(6))
                           .ReturnsAsync(existingBudget);
            _mockRepository.Setup(r => r.SaveChanges())
                           .Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.CreateBudget(It.IsAny<Budget>()))
                           .ReturnsAsync((Budget b) => b);

            // Act
            var result = await _budgetService.CreateBudget(inputBudget);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.Amount);
            _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateBudget_ShouldCreateNewBudget_WhenNoneExists()
        {
            // Arrange
            var inputBudget = new Budget { Amount = 800 };
            Budget capturedBudget = null;

            _mockRepository.Setup(r => r.GetTotalBudgetMonth(6))
                           .ReturnsAsync((Budget)null);

            _mockRepository.Setup(r => r.CreateBudget(It.IsAny<Budget>()))
                           .Callback<Budget>(b => capturedBudget = b)
                           .ReturnsAsync((Budget b) => b);

            // Act
            var result = await _budgetService.CreateBudget(inputBudget);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(800, result.Amount);
            Assert.Equal(6, result.CategoryId);
            Assert.Equal(DateTime.Now.Year, result.Year);
            Assert.Equal(DateTime.Now.Month, result.Month);
            Assert.Equal(result, capturedBudget); // ensures the correct object was saved
            _mockRepository.Verify(r => r.SaveChanges(), Times.Never);
        }
    }
}
