namespace BudgetAPI.Database;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Budget Budget { get; set; }
    public Guid BudgetId { get; set; }
    public User CreatedBy { get; set; }
    public string CreatedById { get; set; }
}