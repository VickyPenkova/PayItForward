namespace PayItForward.Common
{
    using Microsoft.Extensions.Configuration;

    public class ConnectionStringResolver
    {
        public string GetConnectionString()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json", optional: false);
            var configuration = configBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return connectionString;

        }
    }
}
