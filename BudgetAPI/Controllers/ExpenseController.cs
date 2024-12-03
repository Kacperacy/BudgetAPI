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
public class ExpenseController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ExpenseController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        return await _context.Expenses.Where(x => x.User.Id == user.Id).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetExpense(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);

        if (expense == null) return NotFound();

        if (expense.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        return expense;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutExpense(Expense expense)
    {
        if (expense.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        _context.Entry(expense).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Expense>> PostExpense(CreateExpense expense)
    {
        var category = await _context.Categories.FindAsync(expense.CategoryId);

        if (category == null) return BadRequest();

        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        var newExpense = new Expense
        {
            Amount = expense.Amount,
            Date = expense.Date.ToUniversalTime(),
            Description = expense.Description,
            User = user,
            Category = category
        };

        _context.Expenses.Add(newExpense);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetExpense", new { id = newExpense.Id }, newExpense);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);

        if (expense == null) return NotFound();

        if (expense.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        _context.Expenses.Remove(expense);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}