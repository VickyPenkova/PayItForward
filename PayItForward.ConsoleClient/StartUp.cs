namespace PayItForward.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using PayItForward.Common;
    using PayItForward.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var connectionStringResolver = new ConnectionStringResolver();
            var connectionString = connectionStringResolver.GetConnectionString();

            services.AddDbContext<PayItForwardDbContext>(options =>
              options.UseSqlServer(connectionString));

            services.AddIdentity<PayItForward.Data.Models.User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<PayItForwardDbContext>()
                .AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();

            // Seeding data from database
            InitializeData(serviceProvider);

            // Print dbusers info
            PrintData(serviceProvider.GetService<PayItForwardDbContext>());
        }

        private static void InitializeData(ServiceProvider serviceProvider)
        {
            DbInitializer initializer = new DbInitializer(serviceProvider);
            initializer.Initialize();
        }

        private static void PrintData(PayItForwardDbContext context)
        {
            IConsoleWrapper consoleWrapper = new ConsoleWrapper();

            List<Logger> loggers = new List<Logger>()
             {
                new BasicLoggerInfo("I am BasicLoggerInfo", consoleWrapper),
                new ColorfulLoggerInfo("ColorfulLoggerInfo", ConsoleColor.Blue, consoleWrapper),
                new DetailedLoggerInfo("DetailedLoggerInfo", consoleWrapper)
             };

            // Logging data from database
            var usersFromdb = context.Users.ToList();
            List<ILoggable> localUsers = new List<ILoggable>();

            foreach (var user in usersFromdb)
            {
                localUsers.Add(new User(
                        user.FirstName,
                        user.LastName,
                        user.Id));
                if (localUsers.Count == 3)
                {
                    break;
                }
            }

            foreach (var logger in loggers)
            {
                logger.PrintInfoList(localUsers);
            }
        }
    }
}
