using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Database;

public class DbContext : IdentityDbContext<User>
{
   public DbContext(DbContextOptions<DbContext> options) : base(options)
   {
   } 
}