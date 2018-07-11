namespace PayItForward.ConsoleClient
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using PayItForward.Data;

    public class PayItForwardContextFactory : IDesignTimeDbContextFactory<PayItForwardDbContext>
    {
        public PayItForwardDbContext CreateDbContext()
        {
            return this.CreateDbContext(null);
        }

        public PayItForwardDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json", optional: false);

            var configuration = configBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<PayItForwardDbContext>();
            builder.UseSqlServer(connectionString);

            return new PayItForwardDbContext(builder.Options);
        }
    }
}
