namespace BudgetAPI.Database.Dto;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public UserDto User { get; set; }
    public CategoryDto? Category { get; set; }
    public Guid BudgetId { get; set; }
}