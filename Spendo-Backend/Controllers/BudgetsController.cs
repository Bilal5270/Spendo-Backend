using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;

namespace Spendo_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // ✅ Markeert deze controller als een API-controller
    public class BudgetsController : ControllerBase
    {
        private readonly SpendoContext _context;

        public BudgetsController(SpendoContext context)
        {
            _context = context;
        }

        // ✅ GET: api/budgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets()
        {
            return await _context.Budgets.ToListAsync();
        }

        // ✅ GET: api/budgets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }
            return Ok(budget);
        }

        [HttpGet("total/{categoryId}")]
        // ✅ GET: api/budgets/total/{categoryId}
        public async Task<ActionResult<decimal>> GetTotalBudget(int categoryId)
        {
            var generalBudget = await _context.Budgets
                .Where(b => b.CategoryId == categoryId && b.Year == DateTime.Now.Year && b.Month == DateTime.Now.Month)
                .FirstOrDefaultAsync();

            if (generalBudget == null)
            {
                return NotFound("Budget voor deze categorie niet gevonden.");
            }
            return Ok(generalBudget);
        }

        // ✅ GET: api/budgets/remaining/{categoryId}
        [HttpGet("remaining/{categoryId}")]
        public async Task<ActionResult<decimal>> GetRemainingBudget(int categoryId)
        {
            var generalBudget = await _context.Budgets
                .Where(b => b.CategoryId == categoryId && b.Year == DateTime.Now.Year && b.Month == DateTime.Now.Month)
                .FirstOrDefaultAsync();

            if (generalBudget == null)
            {
                return NotFound("Budget voor deze categorie niet gevonden.");
            }

            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var totalExpenses = await _context.Transactions
                .Where(t => t.Type == "Expense")
                .SumAsync(t => t.Amount);

            // Bereken de totale uitgaven
            var remainingBudget = generalBudget.Amount - totalExpenses;

            return Ok(remainingBudget);
        }

        // ✅ POST: api/budgets
        [HttpPost]
        public async Task<ActionResult<Budget>> CreateBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBudget), new { id = budget.BudgetId }, budget);
        }

        // ✅ PUT: api/budgets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, Budget budget)
        {
            if (id != budget.BudgetId)
            {
                return BadRequest();
            }

            _context.Entry(budget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Budgets.Any(e => e.BudgetId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // ✅ DELETE: api/budgets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
