using System.Security.Claims;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public BudgetController(ApiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDto>>> GetBudgets()
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        var budgets = await _context.Budgets
            .Include(p => p.User)
            .Include(p => p.Expenses)
            .Where(x => x.User.Id == user.Id)
            .ToListAsync();
        
        var budgetDtos = _mapper.Map<List<BudgetDto>>(budgets);
        
        return budgetDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDto>> GetBudget(Guid id)
    {
        var budget = await _context.Budgets
            .Include(p => p.User)
            .Include(p => p.Expenses)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (budget == null) return NotFound();

        if (budget.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        var budgetDto = _mapper.Map<BudgetDto>(budget);
        
        return budgetDto;
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