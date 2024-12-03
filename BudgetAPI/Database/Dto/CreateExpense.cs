namespace BudgetAPI.Database.Dto;

public class CreateExpense
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid BudgetId { get; set; }
}