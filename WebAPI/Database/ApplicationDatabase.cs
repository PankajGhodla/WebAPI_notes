using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Database
{
    public class ApplicationDatabase : IdentityDbContext
    {
        public ApplicationDatabase(DbContextOptions<ApplicationDatabase> options): base(options)
        {
            
        }

        public DbSet<LogInModel> LogIn { get; set; }
        public DbSet<SignInModel> SignIn { get; set; }
    }
}