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
   public DbSet<UserBudget> UserBudgets { get; set; }

   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
      
      builder.HasDefaultSchema("identity");

      builder.Entity<UserBudget>(entity =>
      {
         entity.HasKey(e => e.Id);

         entity.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

         entity.HasOne(e => e.Budget)
            .WithMany(b => b.UserBudgets)
            .HasForeignKey(e => e.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);

         entity.HasIndex(e => new { e.UserId, e.BudgetId }).IsUnique();
      });
   }
}