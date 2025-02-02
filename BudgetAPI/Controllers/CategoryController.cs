using System.Security.Claims;
using AutoMapper;
using BudgetAPI.Database;
using BudgetAPI.Database.Dto;
using BudgetAPI.Database.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Controllers;

[Route("api/budgets/{budgetId}/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public CategoryController(ApiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(Guid budgetId)
    {
        var budget = await _context.Budgets
            .Include(b => b.UserBudgets)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null) return NotFound();

        // Check if user has access to this budget
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userBudget = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == userId);
        if (userBudget == null) return Forbid();

        var categories = await _context.Categories
            .Include(c => c.CreatedBy)
            .Where(x => x.BudgetId == budgetId)
            .ToListAsync();

        return Ok(_mapper.Map<List<CategoryDto>>(categories));
    }

    [HttpGet("dropdown")]
    public async Task<ActionResult<IEnumerable<CategoryDropdownDto>>> GetCategoriesForDropdown(Guid budgetId)
    {
        var budget = await _context.Budgets
            .Include(b => b.UserBudgets)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null) return NotFound();

        // Check if user has access to this budget
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userBudget = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == userId);
        if (userBudget == null || userBudget.Role == BudgetRole.Viewer) return Forbid();

        var categories = await _context.Categories
            .Where(x => x.BudgetId == budgetId)
            .OrderBy(x => x.Name)
            .Select(x => new CategoryDropdownDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(Guid budgetId, CreateCategory dto)
    {
        var budget = await _context.Budgets
            .Include(b => b.UserBudgets)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null) return NotFound();

        // Check if user has access and can manage budget
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userBudget = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == userId);
        if (userBudget == null || userBudget.Role == BudgetRole.Viewer) 
            return Forbid();

        var newCategory = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            BudgetId = budgetId,
            CreatedById = userId
        };

        _context.Categories.Add(newCategory);
        await _context.SaveChangesAsync();

        await _context.Entry(newCategory)
            .Reference(c => c.CreatedBy)
            .LoadAsync();

        return CreatedAtAction(nameof(GetCategories), 
            new { budgetId }, 
            _mapper.Map<CategoryDto>(newCategory));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid budgetId, Guid id)
    {
        var budget = await _context.Budgets
            .Include(b => b.UserBudgets)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null) return NotFound();

        // Check if user has access and can manage budget
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userBudget = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == userId);
        if (userBudget == null || userBudget.Role == BudgetRole.Viewer) 
            return Forbid();

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        if (category.BudgetId != budgetId) return BadRequest();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}