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
    
}
}
