using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;
using Spendo_Backend.Services;

namespace Spendo_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // ✅ Markeert deze controller als een API-controller
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _service;

        public BudgetsController(IBudgetService service)
        {
            _service = service;
        }

        [HttpGet("total/{categoryId}")]
        // ✅ GET: api/budgets/total/{categoryId}
        public async Task<ActionResult<decimal>> GetTotalBudget(int categoryId)
        {
            var totalBudget = await _service.GetTotalBudget(categoryId);
            return Ok(totalBudget);
        }

        // ✅ GET: api/budgets/remaining/{categoryId}
        [HttpGet("remaining/{categoryId}")]
        public async Task<ActionResult<decimal>> GetRemainingBudget(int categoryId)
        {
            var remainingBudget = await _service.GetRemainingBudget(categoryId);

            return Ok(remainingBudget);
        }
    }
}
