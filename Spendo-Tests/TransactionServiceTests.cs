using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Spendo_Backend.Services;
using Spendo_Backend.Models;
using Spendo_Backend.Repositories;
namespace Spendo_Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ISpendoRepository> _mockRepo;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mockRepo = new Mock<ISpendoRepository>();
            _transactionService = new TransactionService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllTransactions_ReturnsListOfTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Amount = 50, Description = "Boodschappen", TransactionDate = DateTime.Now },
                new Transaction { Amount = 100, Description = "Huur", TransactionDate = DateTime.Now }
            };

            _mockRepo.Setup(r => r.GetAllTransactionsAsync())
                     .ReturnsAsync(transactions);

            // Act
            var result = await _transactionService.GetAllTransactions();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Boodschappen", result[0].Description);
        }

        [Fact]
        public async Task CreateTransaction_CallsRepositoryAndReturnsCreatedTransaction()
        {
            // Arrange
            var newTransaction = new Transaction
            {
                Amount = 25,
                Description = "Taxi",
                TransactionDate = DateTime.Now
            };

            _mockRepo.Setup(r => r.GetTotalTransactionsDecimalMonth())
                     .ReturnsAsync(100m);

            _mockRepo.Setup(r => r.CreateTransaction(It.IsAny<Transaction>()))
                     .ReturnsAsync((Transaction t) => t);

            // Act
            var result = await _transactionService.CreateTransaction(newTransaction);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Taxi", result.Description);
            Assert.Equal(25, result.Amount);
            _mockRepo.Verify(r => r.CreateTransaction(It.IsAny<Transaction>()), Times.Once);
        }
        [Fact]
        public async Task GetAllConvertedRecurringTransactionsAsync_ReturnsCorrectConvertedAmounts()
        {
            // Arrange
            var recurringTransactions = new List<RecurringTransaction>
    {
        new RecurringTransaction
        {
            RecurringId = 1,
            Amount = 100,
            Description = "Salaris",
            Type = "income",
            RecurrenceInterval = "monthly",
            Category = new Category { Name = "Werk" }
        },
        new RecurringTransaction
        {
            RecurringId = 2,
            Amount = 52,
            Description = "Netflix",
            Type = "expense",
            RecurrenceInterval = "yearly",
            Category = new Category { Name = "Entertainment" }
        }
    };

            _mockRepo.Setup(r => r.GetAllRecurringTransactionsAsync())
                     .ReturnsAsync(recurringTransactions);

            // Act
            var result = await _transactionService.GetAllConvertedRecurringTransactionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var salaris = result.First(r => r.Description == "Salaris");
            Assert.Equal(100, salaris.OriginalAmount);
            Assert.Equal(100, salaris.MonthlyAmount);
            Assert.InRange(salaris.WeeklyAmount, 23.09m, 23.11m); // 100 / 4.33
            Assert.Equal(1200, salaris.YearlyAmount); // 100 * 12

            var netflix = result.First(r => r.Description == "Netflix");
            Assert.Equal(52, netflix.OriginalAmount);
            Assert.InRange(netflix.WeeklyAmount, 1.00m, 1.01m); // 52 / 52
            Assert.InRange(netflix.MonthlyAmount, 4.33m, 4.34m); // 52 / 12
            Assert.Equal(52, netflix.YearlyAmount);
        }

        [Fact]
        public async Task GetAllConvertedRecurringTransactionsAsync_HandlesUnknownInterval()
        {
            // Arrange
            var recurringTransactions = new List<RecurringTransaction>
    {
        new RecurringTransaction
        {
            RecurringId = 3,
            Amount = 100,
            Description = "Onbekend",
            Type = "income",
            RecurrenceInterval = "unknown",
            Category = null
        }
    };

            _mockRepo.Setup(r => r.GetAllRecurringTransactionsAsync())
                     .ReturnsAsync(recurringTransactions);

            // Act
            var result = await _transactionService.GetAllConvertedRecurringTransactionsAsync();

            // Assert
            var item = result.First();
            Assert.Equal(100, item.WeeklyAmount);
            Assert.Equal(100, item.MonthlyAmount);
            Assert.Equal(100, item.YearlyAmount);
        }
        [Fact]
        public async Task CreateRecurringTransactionAsync_ThrowsException_WhenAmountIsZeroOrLess()
        {
            // Arrange
            var invalidTransaction = new RecurringTransaction
            {
                Amount = 0,
                Description = "Test",
                RecurrenceInterval = "monthly"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _transactionService.CreateRecurringTransactionAsync(invalidTransaction));

            Assert.Equal("Bedrag moet groter dan 0 zijn.", ex.Message);
        }

        [Fact]
        public async Task CreateRecurringTransactionAsync_SetsDefaultRecurrenceInterval_WhenEmpty()
        {
            // Arrange
            var recurringTransaction = new RecurringTransaction
            {
                Amount = 100,
                Description = "Test zonder interval",
                RecurrenceInterval = null
            };

            _mockRepo.Setup(r => r.CreateRecurringTransaction(It.IsAny<RecurringTransaction>()))
                     .ReturnsAsync((RecurringTransaction rt) => rt);

            // Act
            var result = await _transactionService.CreateRecurringTransactionAsync(recurringTransaction);

            // Assert
            Assert.Equal("monthly", result.RecurrenceInterval);
            Assert.Equal(100, result.Amount);
            _mockRepo.Verify(r => r.CreateRecurringTransaction(It.IsAny<RecurringTransaction>()), Times.Once);
        }

        [Fact]
        public async Task CreateRecurringTransactionAsync_CallsRepository_AndReturnsCreatedTransaction()
        {
            // Arrange
            var recurringTransaction = new RecurringTransaction
            {
                Amount = 150,
                Description = "Maandelijkse test",
                RecurrenceInterval = "yearly"
            };

            _mockRepo.Setup(r => r.CreateRecurringTransaction(It.IsAny<RecurringTransaction>()))
                     .ReturnsAsync((RecurringTransaction rt) => rt);

            // Act
            var result = await _transactionService.CreateRecurringTransactionAsync(recurringTransaction);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(150, result.Amount);
            Assert.Equal("yearly", result.RecurrenceInterval);
            _mockRepo.Verify(r => r.CreateRecurringTransaction(It.IsAny<RecurringTransaction>()), Times.Once);
        }
    }
}

