using System.Security.Claims;
using AutoMapper;
using BudgetAPI.Database;
using BudgetAPI.Database.Dto;
using BudgetAPI.Database.Enums;
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
            .Include(p => p.UserBudgets)
            .ThenInclude(p => p.User)
            .Where(p => p.UserBudgets.Any(ub => ub.UserId == user.Id))
            .ToListAsync();
        
        var budgetDtos = _mapper.Map<List<BudgetDto>>(budgets);

        foreach (var budgetDto in budgetDtos)
        {
            var userBudget = budgets
                .First(p => p.Id == budgetDto.Id)
                .UserBudgets
                .First(p => p.UserId == user.Id);

            budgetDto.CurrentUserRole = userBudget.Role;
        }
        
        return budgetDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDto>> GetBudget(Guid id)
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();
        
        var budget = await _context.Budgets
            .Include(p => p.User)
            .Include(p => p.UserBudgets)
            .ThenInclude(p => p.User)
            .Include(p => p.Expenses)
            .ThenInclude(p => p.Category)
            .Where(p => p.UserBudgets.Any(ub => ub.UserId == user.Id))
            .FirstOrDefaultAsync(p => p.Id == id);

        if (budget == null) return NotFound();

        var budgetDto = _mapper.Map<BudgetDto>(budget);
        
        var userBudget = budget.UserBudgets.First(p => p.UserId == user.Id);
        
        budgetDto.CurrentUserRole = userBudget.Role;
        
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
    public async Task<ActionResult<BudgetDto>> PostBudget(CreateBudget budget)
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        var newBudget = new Budget
        {
            Name = budget.Name,
            Amount = budget.Amount,
            StartDate = budget.StartDate.ToUniversalTime(),
            EndDate = budget.EndDate.ToUniversalTime(),
            User = user,
        };
        
        var userBudget = new UserBudget
        {
            UserId = user.Id,
            Budget = newBudget,
            Role = BudgetRole.Owner
        };

        _context.Budgets.Add(newBudget);
        _context.UserBudgets.Add(userBudget);

        await _context.SaveChangesAsync();
        
        var budgetDto = _mapper.Map<BudgetDto>(newBudget);

        return CreatedAtAction("GetBudget", new { id = newBudget.Id }, budgetDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(Guid id)
    {
        var budget = await _context.Budgets.FindAsync(id);

        if (budget == null) return NotFound();

        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (user == null) return Unauthorized();
        
        var userBudget = await _context.UserBudgets
            .FirstOrDefaultAsync(ub => ub.UserId == user.Id && ub.BudgetId == id);
        
        if (userBudget == null || userBudget.Role != BudgetRole.Owner) return Forbid();

        _context.Budgets.Remove(budget);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}