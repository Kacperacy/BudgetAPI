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
public class ExpenseController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseController(ApiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        var expenses = await _context.Expenses
            .Include(p => p.User)
            .Where(x => x.User.Id == user.Id)
            .ToListAsync();
        
        var expenseDtos = _mapper.Map<List<ExpenseDto>>(expenses);
        
        return expenseDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);

        if (expense == null) return NotFound();

        if (expense.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        var expenseDto = _mapper.Map<ExpenseDto>(expense);
        
        return expenseDto;
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
    public async Task<ActionResult<ExpenseDto>> PostExpense(CreateExpense expense)
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
    
        if (user == null) return Unauthorized();
    
        var category = await _context.Categories.FindAsync(expense.CategoryId);
    
        var budget = await _context.Budgets
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == expense.BudgetId);
    
        if (budget == null) return BadRequest();
    
        if (user.Id != budget.User.Id) return Unauthorized();
    
        var newExpense = new Expense
        {
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date.ToUniversalTime(),
            User = user,
            Category = category,
            Budget = budget
        };
        
        _context.Expenses.Add(newExpense);

        await _context.SaveChangesAsync();
        
        var expenseDto = _mapper.Map<ExpenseDto>(newExpense);

        return CreatedAtAction("GetExpense", new { id = expenseDto.Id }, expenseDto);
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