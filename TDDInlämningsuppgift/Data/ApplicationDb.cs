using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDInlämningsuppgift.Model;

namespace TDDInlämningsuppgift.Data
{
    public class ApplicationDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posters { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=FacebookDb;Trusted_Connection=True;");
        }
    }
    public class DataInitializer
    {
        public static void SeedData()
        {
            var database = new ApplicationDb();
            if (!database.Users.Any(u => u.Username == "Alice123" || u.Username == "Bob123" || u.Username == "Charlie123"))
            {
                database.Users.Add(new User { Name = "Alice", Username = "Alice123", Password = "1234" });
                database.Users.Add(new User { Name = "Bob", Username = "Bob123", Password = "5678" });
                database.Users.Add(new User { Name = "Charlie", Username = "Charlie123", Password = "1546" });
                database.Users.Add(new User { Name = "Mallory", Username = "Mallory123", Password = "8456" });
                database.SaveChanges();
            }
        }
    }
}
