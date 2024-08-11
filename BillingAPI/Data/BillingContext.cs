using Microsoft.EntityFrameworkCore;
using BillingAPI.Models;

namespace BillingAPI.Data
{
    public class BillingContext : DbContext
    {
        public BillingContext(DbContextOptions<BillingContext> options) : base(options)
        {
        }

        public DbSet<Billing> Billings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;Database=restfulapi;User=root;Password=;", 
                    new MySqlServerVersion(new Version(8, 0, 23)));
            }
        }
    }
}
