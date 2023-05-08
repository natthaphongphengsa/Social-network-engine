using Microsoft.EntityFrameworkCore;
using SocialNetworkEngine.Model;
using System.Linq;

namespace SocialNetworkEngine.Data
{
    public class ApplicationDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posters { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SocialNetworkDb;Trusted_Connection=True;");
        }
    }
    public class DataInitializer
    {
        public static void SeedData()
        {
            var database = new ApplicationDb();
            if ((database.Users.Any(u => u.Username == "Alice") && database.Users.Any(u => u.Username == "Bob") && database.Users.Any(u => u.Username == "Charlie") && database.Users.Any(u => u.Username == "Mallory")) == false)
            {
                database.Users.Add(new User { Username = "Alice", Password = "1234" });
                database.Users.Add(new User { Username = "Bob", Password = "5678" });
                database.Users.Add(new User { Username = "Charlie", Password = "1546" });
                database.Users.Add(new User { Username = "Mallory", Password = "8456" });
                database.SaveChanges();
            }
        }
    }
}
