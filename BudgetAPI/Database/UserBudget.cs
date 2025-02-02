using BudgetAPI.Database.Enums;

namespace BudgetAPI.Database;

public class UserBudget
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Guid BudgetId { get; set; }
    public BudgetRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual User User { get; set; }
    public virtual Budget Budget { get; set; }
}