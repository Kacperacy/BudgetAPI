using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Database;

public class ApiDbContext : IdentityDbContext<User>
{
   public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
   {
   }
   
   public DbSet<Budget> Budgets { get; set; }
   public DbSet<Expense> Expenses { get; set; }
   public DbSet<Category> Categories { get; set; }

   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
      
      builder.HasDefaultSchema("identity");
   }
}