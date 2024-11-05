namespace BudgetAPI.Database;

public class Expense
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    
    public User User { get; set; }
    public Category Category { get; set; }
    public Budget Budget { get; set; }
}