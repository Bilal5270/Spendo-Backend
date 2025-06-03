using Microsoft.AspNetCore.Mvc;
using Spendo_Backend.Models;
using Spendo_Backend.Services;

namespace Spendo_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _service;

        public BudgetsController(IBudgetService service)
        {
            _service = service;
        }

        // ✅ GET: api/budgets/total/{categoryId}
        [HttpGet("total/{categoryId}")]
        public async Task<ActionResult<decimal?>> GetTotalBudget(int categoryId)
        {
            var totalBudget = await _service.GetTotalBudget(categoryId);

            if (totalBudget == null)
            {
                // Geef null terug zodat de frontend weet dat er geen budget is
                return Ok((decimal?)null);
            }

            return Ok(totalBudget.Amount);
        }

        // ✅ GET: api/budgets/remaining/{categoryId}
        [HttpGet("remaining/{categoryId}")]
        public async Task<ActionResult<decimal?>> GetRemainingBudget(int categoryId)
        {
            var remainingBudget = await _service.GetRemainingBudget(categoryId);

            if (remainingBudget == null)
            {
                return Ok((decimal?)null);
            }

            return Ok(remainingBudget);
        }

        // ✅ POST: api/budgets
        [HttpPost]
        public async Task<ActionResult<Budget>> CreateBudget([FromBody] Budget budget)
        {
            if (budget == null)
            {
                return BadRequest("Budget mag niet null zijn.");
            }

            var createdBudget = await _service.CreateBudget(budget);

            return CreatedAtAction(nameof(GetTotalBudget), new { categoryId = createdBudget.CategoryId }, createdBudget);
        }
    }
}
