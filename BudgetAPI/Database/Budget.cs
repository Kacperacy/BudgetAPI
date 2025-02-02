namespace BudgetAPI.Database;

public class Budget
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public User User { get; set; }
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<UserBudget> UserBudgets { get; set; } = new List<UserBudget>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}