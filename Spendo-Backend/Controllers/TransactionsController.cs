using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;
using Spendo_Backend.Services;

namespace Spendo_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // ✅ Markeert deze controller als een API-controller
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        [HttpGet("")]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactions();
            return Ok(transactions);
        }

        [HttpPost("")]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Transaction cannot be null");
            }
            var createdTransaction = await _transactionService.CreateTransaction(transaction);
            return CreatedAtAction(nameof(GetAllTransactions), new { id = createdTransaction.TransactionId }, createdTransaction);
        }

    }
}
