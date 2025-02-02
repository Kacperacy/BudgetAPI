using System.Security.Claims;
using AutoMapper;
using BudgetAPI.Database;
using BudgetAPI.Database.Dto;
using BudgetAPI.Database.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Controllers;

[Route("api/budgets/{budgetId}/users")]
[ApiController]
[Authorize]
public class BudgetUsersController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly IMapper _mapper;

    public BudgetUsersController(ApiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserBudgetDto>>> GetBudgetUsers(Guid budgetId)
    {
        var budget = await _context.Budgets
            .Include(b => b.UserBudgets)
                .ThenInclude(ub => ub.User)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null)
            return NotFound();

        // Check if user has access to this budget
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userBudget = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == userId);
        if (userBudget == null || userBudget.Role != BudgetRole.Owner)
            return Forbid();

        return Ok(_mapper.Map<List<UserBudgetDto>>(budget.UserBudgets));
    }

    [HttpPost]
    public async Task<ActionResult<UserBudgetDto>> AddUserToBudget(Guid budgetId, AddUserToBudgetDto dto)
    {
        var budget = await _context.Budgets
            .Include(b => b.UserBudgets)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null)
            return NotFound();

        // Check if requesting user is Owner
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userBudget = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == userId);
        if (userBudget?.Role != BudgetRole.Owner)
            return Forbid();
        
        var newUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (newUser == null)
            return NotFound();

        var newUserBudget = new UserBudget
        {
            UserId = newUser.Id,
            BudgetId = budgetId,
            Role = dto.Role
        };

        _context.UserBudgets.Add(newUserBudget);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBudgetUsers), new { budgetId }, _mapper.Map<UserBudgetDto>(newUserBudget));
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> RemoveUserFromBudget(Guid budgetId, string userId)
    {
        var budget = await _context.Budgets
            .Include(b => b.UserBudgets)
            .FirstOrDefaultAsync(b => b.Id == budgetId);

        if (budget == null)
            return NotFound();

        // Check if requesting user is Owner
        var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var requestingUserBudget = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == requestingUserId);
        if (requestingUserBudget?.Role != BudgetRole.Owner)
            return Forbid();

        var userBudgetToRemove = budget.UserBudgets.FirstOrDefault(ub => ub.UserId == userId);
        if (userBudgetToRemove == null)
            return NotFound();

        _context.UserBudgets.Remove(userBudgetToRemove);
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 