using Microsoft.EntityFrameworkCore;

namespace WebAPI.Database
{
    public class ApplicationDatabase : DbContext
    {
        public ApplicationDatabase(DbContextOptions<ApplicationDatabase> options): base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>()
            .Property(o => o.UserID)
            .ValueGeneratedOnAdd();


            modelBuilder.Entity<UserNote>()
            .Property(o => o.NoteID)
            .ValueGeneratedOnAdd();


        }
        public DbSet<User> User { get; set; }
        public DbSet<UserNote> UserNote { get; set; }
    }
}