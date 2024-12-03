using System.Security.Claims;
using BudgetAPI.Database;
using BudgetAPI.Database.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly ApiDbContext _context;

    public BudgetController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets()
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        return await _context.Budgets.Where(x => x.User.Id == user.Id).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Budget>> GetBudget(Guid id)
    {
        var budget = await _context.Budgets.FindAsync(id);

        if (budget == null) return NotFound();

        if (budget.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        return budget;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBudget(Budget budget)
    {
        if (budget.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        _context.Entry(budget).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Budget>> PostBudget(CreateBudget budget)
    {
        var category = await _context.Categories.FindAsync(budget.CategoryId);

        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        var newBudget = new Budget
        {
            Name = budget.Name,
            Amount = budget.Amount,
            StartDate = budget.StartDate.ToUniversalTime(),
            EndDate = budget.EndDate.ToUniversalTime(),
            User = user,
            Category = category
        };

        _context.Budgets.Add(newBudget);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBudget", new { id = newBudget.Id }, newBudget);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(Guid id)
    {
        var budget = await _context.Budgets.FindAsync(id);

        if (budget == null) return NotFound();

        if (budget.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        _context.Budgets.Remove(budget);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}