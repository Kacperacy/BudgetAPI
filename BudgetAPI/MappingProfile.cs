using AutoMapper;

namespace BudgetAPI;

public class MappingProfile: Profile
{
    public  MappingProfile()
    {
        CreateMap<Database.Budget, Database.Dto.BudgetDto>();
        CreateMap<Database.Expense, Database.Dto.ExpenseDto>()
            .ForMember(p => p.BudgetId , opt => opt.MapFrom(src => src.Budget.Id));
        CreateMap<Database.User, Database.Dto.UserDto>();
        // CreateMap<Database.Category, Database.Dto.CategoryDto>();
    }
}