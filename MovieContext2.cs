using Microsoft.EntityFrameworkCore;

namespace MoviesApp
{
    public class MoviesContext : DbContext
    {
        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=movies.db");
        }
    }
}
