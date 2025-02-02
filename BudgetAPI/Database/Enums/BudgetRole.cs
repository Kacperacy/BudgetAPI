namespace BudgetAPI.Database.Enums;

public enum BudgetRole
{
    Owner,      // Can do everything
    Manager,    // Can edit budget and expenses
    Viewer      // Can only view
}