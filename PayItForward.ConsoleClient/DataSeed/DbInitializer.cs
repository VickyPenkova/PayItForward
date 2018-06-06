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
    using Dbmodel = PayItForward.Data.Models;

    public class DbInitializer
    {
        private IServiceProvider serviceProvider;
        private PayItForwardDbContext context;

        public DbInitializer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.context = this.serviceProvider.GetRequiredService<PayItForwardDbContext>();
        }

        public void Initialize()
        {
            this.context.Database.Migrate();

            this.AddRoles();

            this.SeedUsers();

            this.SeedCategories();

            this.SeedStories();

            this.SeedDonations();
        }

        private void AddRoles()
        {
            var roleManager = this.serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleCheck = roleManager.RoleExistsAsync(GlobalConstants.AdminRole).Result;

            if (!roleCheck)
            {
                roleManager.CreateAsync(new IdentityRole(GlobalConstants.AdminRole)).Wait();
            }

            roleCheck = roleManager.RoleExistsAsync(GlobalConstants.UserRole).Result;
            if (!roleCheck)
            {
                roleManager.CreateAsync(new IdentityRole(GlobalConstants.UserRole)).Wait();
            }
        }

        private void SeedUsers()
        {
            if (this.context.Users.Any())
            {
                return;
            }

            var userManager = this.serviceProvider.GetRequiredService<UserManager<Dbmodel.User>>();
            var adminUser = new Dbmodel.User
            {
                UserName = "Aleks",
                Email = "aleks@gmail.com",
                FirstName = "Aleksandra",
                LastName = "Stoicheva",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = userManager.CreateAsync(adminUser, "qwerty123@").Result;
            userManager.AddToRoleAsync(adminUser, GlobalConstants.AdminRole).Wait();

            // The security stamp is used to invalidate a users login cookie and force them to re-login.
            var users = new List<Dbmodel.User>()
            {
                new Dbmodel.User
                {
                    UserName = "peter",
                    Email = "peter@gmail.com",
                    FirstName = "Peter",
                    LastName = "Petkov",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new Dbmodel.User
                {
                    UserName = "george",
                    Email = "george@gmail.com",
                    FirstName = "George",
                    LastName = "Mingle",
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            };

            foreach (var user in users)
            {
                userManager.CreateAsync(user, "qwerty123@").Wait();
                userManager.AddToRoleAsync(user, GlobalConstants.UserRole).Wait();
            }
        }

        private void SeedCategories()
        {
            // if there are categories in the database do not seed more
            if (this.context.Categories.Any())
            {
                return;
            }

            List<Dbmodel.Category> categories = new List<Dbmodel.Category>()
            {
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryEducation,
                    IsRemoved = false
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryHealth,
                    IsRemoved = false
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryCharity,
                    IsRemoved = false
                }
            };

            this.context.Categories.AddRange(categories);
            this.context.SaveChanges();
        }

        private void SeedStories()
        {
            if (this.context.Stories.Any())
            {
                return;
            }

            List<Dbmodel.Category> categories = this.context.Categories
                .Where(c =>
                        c.Name == GlobalConstants.CategoryHealth || c.Name == GlobalConstants.CategoryCharity || c.Name == GlobalConstants.CategoryEducation)
                .ToList();

            List<Dbmodel.User> users = this.context.Users
                .Where(u =>
                        u.FirstName == "Aleksandra" || u.FirstName == "Peter" || u.FirstName == "George")
                        .ToList();

            List<Dbmodel.Story> stories = new List<Dbmodel.Story>()
            {
                new Dbmodel.Story
                {
                    Title = "Help me!",
                    IsClosed = false,
                    User = users.FirstOrDefault(u => u.FirstName == "Aleksandra"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryHealth),
                    IsRemoved = false,
                    GoalAmount = 1500,
                    IsAccepted = true,
                    CollectedAmount = 0,
                    ExpirationDate = new DateTime(2018, 9, 9, 16, 5, 7, 123)
                },
                new Dbmodel.Story
                {
                    Title = "Education support",
                    IsClosed = false,
                    User = users.FirstOrDefault(u => u.FirstName == "Peter"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryEducation),
                    IsRemoved = false,
                    GoalAmount = 900,
                    IsAccepted = true,
                    DateCreated = DateTime.Now,
                    CollectedAmount = 30,
                    ExpirationDate = new DateTime(2018, 9, 9, 16, 5, 7, 123)
                },
                new Dbmodel.Story
                {
                    Title = "Sponsor me!",
                    IsClosed = false,
                    User = users.FirstOrDefault(u => u.FirstName == "George"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryCharity),
                    IsRemoved = false,
                    GoalAmount = 700,
                    IsAccepted = true,
                    DateCreated = DateTime.Now,
                    CollectedAmount = 80,
                    ExpirationDate = new DateTime(2018, 9, 9, 16, 5, 7, 123)
                }
            };

            this.context.Stories.AddRange(stories);

            this.context.SaveChanges();
        }

        private void SeedDonations()
        {
            if (this.context.Donations.Any())
            {
                return;
            }

            List<Dbmodel.User> users = this.context.Users
                .Where(u =>
                        u.FirstName == "Aleksandra" || u.FirstName == "Peter" || u.FirstName == "George")
                        .ToList();

            List<Dbmodel.Donation> donations = new List<Dbmodel.Donation>()
            {
                new Dbmodel.Donation
                {
                    Amount = 300,
                    User = users.FirstOrDefault(u => u.FirstName == "Aleksandra"),
                    StoryId = this.context.Stories.FirstOrDefault(c => c.Title == "Sponsor me!").Id
                },
                new Dbmodel.Donation
                {
                    Amount = 800,
                    User = users.FirstOrDefault(u => u.FirstName == "Peter"),
                    StoryId = this.context.Stories.FirstOrDefault(c => c.Title == "Help me!").Id
                },
                new Dbmodel.Donation
                {
                    Amount = 3006,
                    User = users.FirstOrDefault(u => u.FirstName == "George"),
                    StoryId = this.context.Stories.FirstOrDefault(c => c.Title == "Education support").Id
                }
            };

            this.context.Donations.AddRange(donations);

            this.context.SaveChanges();
        }
    }
}
