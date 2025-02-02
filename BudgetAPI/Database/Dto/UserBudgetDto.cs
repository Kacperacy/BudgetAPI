using BudgetAPI.Database.Enums;

namespace BudgetAPI.Database.Dto;

public class UserBudgetDto
{
    public Guid Id { get; set; }
    public UserDto User { get; set; }
    public BudgetRole Role { get; set; }
}

public class AddUserToBudgetDto
{
    public string Email { get; set; }
    public BudgetRole Role { get; set; }
} 