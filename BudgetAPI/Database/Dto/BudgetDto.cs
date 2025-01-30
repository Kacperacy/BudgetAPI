namespace BudgetAPI.Database.Dto;

public class BudgetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public UserDto User { get; set; }
    public Category? Category { get; set; }
    public List<ExpenseDto> Expenses { get; set; }
}