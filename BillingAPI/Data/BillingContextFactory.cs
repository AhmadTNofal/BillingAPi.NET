using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BillingAPI.Data
{
    public class BillingContextFactory : IDesignTimeDbContextFactory<BillingContext>
    {
        public BillingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BillingContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=restfulapi;User=root;Password=;", 
                new MySqlServerVersion(new Version(8, 0, 23)));

            return new BillingContext(optionsBuilder.Options);
        }
    }
}
