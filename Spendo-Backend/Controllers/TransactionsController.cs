using Microsoft.AspNetCore.Mvc;
using Spendo_Backend.Models;
using Spendo_Backend.Models.DTO;
using Spendo_Backend.Services;

namespace Spendo_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactions();
            return Ok(transactions);
        }

        [HttpGet("recurring")]
        public async Task<ActionResult<List<RecurringTransactionDTO>>> GetAllRecurringTransactions()
        {
            var transactions = await _transactionService.GetAllConvertedRecurringTransactionsAsync();
            return Ok(transactions);
        }

        [HttpPost("")]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Transactie mag niet null zijn.");
            }

            var createdTransaction = await _transactionService.CreateTransaction(transaction);

            if (createdTransaction == null)
            {
                return StatusCode(500, "Transactie kon niet worden aangemaakt.");
            }

            return CreatedAtAction(nameof(GetAllTransactions), new { id = createdTransaction.TransactionId }, createdTransaction);
        }
    }
}
