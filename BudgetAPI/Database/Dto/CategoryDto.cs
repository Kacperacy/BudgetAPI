namespace BudgetAPI.Database.Dto;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public UserDto CreatedBy { get; set; }
}