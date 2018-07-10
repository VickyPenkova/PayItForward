namespace PayItForward.UnitTests.Web.Controllers.Stubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PayItForward.Models;

    public static class AdminController_Stubs
    {
        public static UserDTO GetTestUserDto(string userId)
        {
            return new UserDTO()
            {
                FirstName = "Test",
                LastName = "Unit",
                Id = userId
            };
        }

        public static List<StoryDTO> GetTestListWIthStoryDtos(Guid storyId, string userId)
        {
            return new List<StoryDTO>()
            {
                new StoryDTO()
                {
                    Title = "Story1",
                    Id = storyId,
                    User = GetTestUserDto(userId)
                }
            };
        }

        public static List<DonationDTO> GetTestListWithDOnationDtos(Guid storyId, string userId)
        {
            return new List<DonationDTO>()
            {
               new DonationDTO()
               {
                   Amount = 10,
                   Donator = GetTestUserDto(userId),
                   Id = Guid.NewGuid(),
                   Story = GetTestListWIthStoryDtos(storyId, userId).FirstOrDefault()
               }
            };
        }
    }
}
