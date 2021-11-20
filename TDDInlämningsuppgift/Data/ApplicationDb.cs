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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=FacebookDb;Trusted_Connection=True;");
        }
    }
}
