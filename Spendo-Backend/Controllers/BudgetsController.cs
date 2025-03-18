using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spendo_Backend.Models;

namespace Spendo_Backend.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly SpendoContext _context;

        public BudgetsController(SpendoContext context)
        {
            _context = context;
        }

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            var spendoContext = _context.Budgets.Include(b => b.Category).Include(b => b.User);
            return View(await spendoContext.ToListAsync());
        }
        [HttpGet("remaining/{categoryId}")]
        public async Task<ActionResult<decimal>> GetRemainingBudget(int categoryId)
        {
            // Haal het algemene budget voor de huidige maand voor de specifieke categorie
            var generalBudget = await _context.Budgets
                .Where(b => b.CategoryId == categoryId && b.Year == DateTime.Now.Year && b.Month == DateTime.Now.Month)
                .FirstOrDefaultAsync();

            if (generalBudget == null)
            {
                return NotFound("Budget voor deze categorie niet gevonden.");
            }

            // Haal de transacties voor de huidige maand op voor die categorie
            var totalExpenses = await _context.Transactions
                .Where(t => t.CategoryId == categoryId && t.Type == "Expense" && t.TransactionDate.Year == DateTime.Now.Year && t.TransactionDate.Month == DateTime.Now.Month)
                .SumAsync(t => t.Amount);

            // Resterend budget = algemeen budget - totale uitgaven
            var remainingBudget = generalBudget.Amount - totalExpenses;

            return Ok(remainingBudget);
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budgets
                .Include(b => b.Category)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Budgetid == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid");
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Budgetid,Userid,Categoryid,Amount,Startdate,Enddate")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", budget.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", budget.Userid);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", budget.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", budget.Userid);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Budgetid,Userid,Categoryid,Amount,Startdate,Enddate")] Budget budget)
        {
            if (id != budget.Budgetid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.Budgetid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", budget.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", budget.Userid);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budgets
                .Include(b => b.Category)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Budgetid == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(int id)
        {
            return _context.Budgets.Any(e => e.Budgetid == id);
        }
    }
}
