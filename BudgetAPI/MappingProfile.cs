using AutoMapper;
using BudgetAPI.Database;
using BudgetAPI.Database.Dto;

namespace BudgetAPI;

public class MappingProfile: Profile
{
    public  MappingProfile()
    {
        CreateMap<Budget, BudgetDto>()
            .ForMember(p => p.CurrentUserRole, opt => opt.Ignore());
        CreateMap<Expense, ExpenseDto>()
            .ForMember(p => p.BudgetId, opt => opt.MapFrom(src => src.Budget.Id));
        CreateMap<User, UserDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CategoryDropdownDto>();
        CreateMap<UserBudget, UserBudgetDto>();
    }
}