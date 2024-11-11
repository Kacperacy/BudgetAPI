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
public class CategoryController : ControllerBase
{
    private readonly ApiDbContext _context;

    public CategoryController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        return await _context.Categories.Where(x => x.User.Id == user.Id).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null) return NotFound();

        if (category.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        return category;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(Category category)
    {
        if (category.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        _context.Entry(category).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(CreateCategory category)
    {
        var user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user == null) return Unauthorized();

        var newCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = category.Name,
            Description = category.Description,
            User = user
        };

        _context.Categories.Add(newCategory);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategory", new { id = newCategory.Id }, newCategory);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null) return NotFound();

        if (category.User.Id != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Unauthorized();

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}