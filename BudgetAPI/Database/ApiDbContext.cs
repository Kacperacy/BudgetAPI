using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Database;

public class ApiDbContext : IdentityDbContext<User>
{
   public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
   {
   }

   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);
      
      builder.HasDefaultSchema("identity");
   }
}