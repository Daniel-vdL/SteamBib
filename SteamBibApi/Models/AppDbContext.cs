using Microsoft.EntityFrameworkCore;
using SteamBibApi.Models;

namespace SteamBibApi.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<App> Apps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=root;" +
                "password=admin123;" +
                "database=SteamBibApi;",
            ServerVersion.Parse("5.7.33-winx64")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
                modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Admin", StatusId = 1, Password = SecureHasher.Hash("1234")},
                new User { Id = 2, Username = "User", StatusId = 0, Password = SecureHasher.Hash("1234")});

                modelBuilder.Entity<App>().HasData(
                new User { Id = 1, Username = "Admin", StatusId = 1, Password = SecureHasher.Hash("1234") },
                new User { Id = 2, Username = "User", StatusId = 0, Password = SecureHasher.Hash("1234") });
        }
    }
}
