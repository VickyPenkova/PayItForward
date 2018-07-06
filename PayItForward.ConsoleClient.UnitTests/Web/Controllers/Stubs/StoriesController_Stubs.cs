namespace PayItForward.UnitTests.Web.Controllers.Stubs
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Models;
    using PayItForward.Web.Models.DonationViewModels;

    public static class StoriesController_Stubs
    {
        public static List<StoryDTO> GetTestStoryDtos()
        {
            return new List<StoryDTO>()
            {
                new StoryDTO
                {
                    Category = new CategoryDTO()
                    {
                        Name = "Health"
                    },
                    CollectedAmount = 3,
                    CreatedOn = DateTime.UtcNow,
                    Description = "Some description",
                    Title = "Title",
                    User = GetTestUserDTO(),
                    Id = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0"),
                    GoalAmount = 30000
                },
                new StoryDTO
                {
                    Category = new CategoryDTO()
                    {
                        Name = "Sport"
                    },
                    CollectedAmount = 3,
                    CreatedOn = DateTime.UtcNow,
                    Description = "Some description",
                    Title = "Title",
                    User = GetTestUserDTO(),
                    Id = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0"),
                    GoalAmount = 30000
                }
            };
        }

        public static UserDTO GetTestUserDTO()
        {
            return new Models.UserDTO()
            {
                Email = "vicky.penkova@gmial.com",
                FirstName = "Viki",
                LastName = "Penkova",
                AvilableMoneyAmount = 10000,
                Id = "alabala"
            };
        }

        public static DonateViewModel GetDonateViewModel()
        {
            return new DonateViewModel()
            {
                Amount = 0,
                CollectedAmount = 3,
                Donator = GetTestUserDTO(),
                GoalAmount = 3000,
                Title = "Title",
                ErrorMessage = "Can not donate!"
            };
        }

        public static List<StoryDTO> GetTestStoryDto()
        {
            return new List<StoryDTO>()
            {
                new StoryDTO
                {
                    Category = new CategoryDTO()
                    {
                        Name = "Health"
                    },
                    CollectedAmount = 0,
                    CreatedOn = DateTime.UtcNow,
                    Description = "Some description",
                    Title = "Title",
                    User = GetTestUserDTO(),
                    Id = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0")
                }
            };
        }
    }
}
