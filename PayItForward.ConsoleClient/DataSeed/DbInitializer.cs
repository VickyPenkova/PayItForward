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
                UserName = "aleks@gmail.com",
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
                    UserName = "peter@gmail.com",
                    Email = "peter@gmail.com",
                    FirstName = "Peter",
                    LastName = "Petkov",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new Dbmodel.User
                {
                    UserName = "george@gmail.com",
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
                    Name = GlobalConstants.CategoryEducation
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryHealth
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryCharity
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryEmergencies
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryAnimals
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryMemorials
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryVolunteer
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategorySports
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryWishes
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryTravel
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryEmergencies
                },
                new Dbmodel.Category
                {
                    Name = GlobalConstants.CategoryCreative
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
                .Where(
                c => c.IsRemoved != true)
                .ToList();

            List<Dbmodel.User> users = this.context.Users
                .Where(
                u => u.Email != null)
                        .ToList();

            List<Dbmodel.Story> stories = new List<Dbmodel.Story>()
            {
                new Dbmodel.Story
                {
                    Title = "Lyme Disease Fundraising",
                    User = users.FirstOrDefault(u => u.Email == "aleks@gmail.com"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryHealth),
                    GoalAmount = 35000,
                    IsAccepted = true,
                    CollectedAmount = 0,
                    ExpirationDate = new DateTime(2018, 9, 9, 16, 5, 7, 123),
                    Description = "At the age of 13, Janie became seriously ill with a mysterious condition " +
                    "that would later be diagnosed as Lyme disease. Unfortunately, the diagnosis came late and " +
                    "conventional treatments of the disease were no longer working. Jamie and her family kept pushing" +
                    " and found a center in California with high success rates for treating advanced cases like Janie's." +
                    " The family took a leap of faith and started a GoFundMe for Janie to help pay for the treatment," +
                    " which would cost upwards of $50K. Within seven days of starting their GoFundMe, the campaign raised $19K and it has shown no signs of slowing down."
                },
                new Dbmodel.Story
                {
                    Title = "Medical School Tuition Fundraising",
                    User = users.FirstOrDefault(u => u.Email == "aleks@gmail.com"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryEducation),
                    GoalAmount = 900,
                    IsAccepted = true,
                    DateCreated = DateTime.Now,
                    CollectedAmount = 30,
                    ExpirationDate = new DateTime(2018, 9, 9, 16, 5, 7, 123),
                    Description = "Dreaming of being a doctor, Dolapo was crushed when she realized she didn’t have" +
                    " enough money to cover her 4th year tuition at the University of Leicester in England. " +
                    "Determined to receive her degree, she turned to GoFundMe to raise the remainder of " +
                    "her tuition and complete her final year of medical school."
                },
                new Dbmodel.Story
                {
                    Title = "#Pizza4Equality help homeless kids",
                    User = users.FirstOrDefault(u => u.Email == "george@gmail.com"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryCharity),
                    GoalAmount = 700,
                    IsAccepted = true,
                    DateCreated = DateTime.Now,
                    CollectedAmount = 80,
                    ExpirationDate = new DateTime(2018, 9, 9, 16, 5, 7, 123),
                    Description = "If $864,000 can be raised for a pizza joint, how much can we raise for homeless youth?" +
                    " I'm an dreamer, but I say yes, I think we can!"
                },
                 new Dbmodel.Story
                {
                    Title = "Brain Tumor Fundraising",
                    User = users.FirstOrDefault(u => u.Email == "george@gmail.com"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryHealth),
                    GoalAmount = 20000,
                    IsAccepted = true,
                    CollectedAmount = 0,
                    ExpirationDate = new DateTime(2018, 9, 9, 16, 5, 7, 123),
                    Description = "In the middle of his studies as a medical student," +
                    " Dave was diagnosed with a life-threatening brain tumor. " +
                    "Even with such a grim prognosis, hope and perseverance were the defining themes of Dave's story." +
                    " With the support of his sister, family and network, his GoFundMe raised $43K within a single month. " +
                    "Dave and his campaign show no signs of slowing down."
                },
                 new Dbmodel.Story
                {
                    Title = "Fundraising for Vet Treatment Costs",
                    User = users.FirstOrDefault(u => u.Email == "peter@gmail.com"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryAnimals),
                    GoalAmount = 7000,
                    IsAccepted = true,
                    CollectedAmount = 0,
                    ExpirationDate = new DateTime(2020, 9, 9, 16, 5, 7, 123),
                    Description = "Ssgt. Caesar is a 4.5-year-old English Bulldog with a lot of American spirit. " +
                    "Not only is he the mascot for a battalion of Vietnam veterans, he also makes regular visits to marines as a service animal. " +
                    "When Ssgt. Caesar's owners discovered he had an enlarged heart, they started a GoFundMe that raised over $7,000 " +
                    "for his long-term treatment."
                },
                 new Dbmodel.Story
                {
                    Title = "Fundraising for Funeral and Cremation Expenses",
                    User = users.FirstOrDefault(u => u.Email == "peter@gmail.com"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryMemorials),
                    GoalAmount = 7000,
                    IsAccepted = true,
                    CollectedAmount = 0,
                    ExpirationDate = new DateTime(2020, 9, 9, 16, 5, 7, 123),
                    Description = "Marissa described her mother as a “stubborn fighter” who refused to let cancer take her " +
                    "away from her children. After being in remission for over a decade, though, the cancer returned in full " +
                    "force and Marissa’s mother passed away. Marissa started a GoFundMe to honor her mother’s wishes of being " +
                    "cremated and having a party to celebrate her life."
                },
                 new Dbmodel.Story
                {
                    Title = "Help my sister get home to mum",
                    User = users.FirstOrDefault(u => u.Email == "george@gmail.com"),
                    Category = categories.FirstOrDefault(c => c.Name == GlobalConstants.CategoryWishes),
                    GoalAmount = 5000,
                    IsAccepted = true,
                    CollectedAmount = 0,
                    ExpirationDate = new DateTime(2020, 6, 9, 16, 5, 7, 123),
                    Description = "The next few months are an important time to be spending with my mother." +
                    " My sister Caitlynne lives in Florida, works a ton and is a badass single mother. She is the only " +
                    "sibling who is too far to drive to spend time. She will kill me if she knows I am doing this but I " +
                    "would like to raise the money to buy her a plane ticket. "
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
                .Where(
                u => u.Email != null)
                        .ToList();

            List<Dbmodel.Donation> donations = new List<Dbmodel.Donation>()
            {
                new Dbmodel.Donation
                {
                    Amount = 300,
                    User = users.FirstOrDefault(u => u.Email == "aleks@gmail.com"),
                    StoryId = this.context.Stories.FirstOrDefault(c => c.Title == "Medical School Tuition Fundraising").Id
                },
                new Dbmodel.Donation
                {
                    Amount = 800,
                    User = users.FirstOrDefault(u => u.Email == "aleks@gmail.com"),
                    StoryId = this.context.Stories.FirstOrDefault(c => c.Title == "Lyme Disease Fundraising").Id
                },
                new Dbmodel.Donation
                {
                    Amount = 3006,
                    User = users.FirstOrDefault(u => u.Email == "peter@gmail.com"),
                    StoryId = this.context.Stories.FirstOrDefault(c => c.Title == "Fundraising for Funeral and Cremation Expenses").Id
                }
            };

            this.context.Donations.AddRange(donations);

            this.context.SaveChanges();
        }
    }
}
