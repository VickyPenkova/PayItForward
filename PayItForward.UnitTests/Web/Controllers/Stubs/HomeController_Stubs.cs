namespace PayItForward.UnitTests.Web.Controllers.Stubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using PayItForward.Models;
    using PayItForward.Web.Models.CategoryViewModels;
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;

    public static class HomeController_Stubs
    {
        public static IndexViewModel GetTestIndexViewModel()
        {
            var basicStoryViewModel = GetTestListWithStoryDtos();
            return new IndexViewModel
            {
                CurrentPage = 1,
                TotalPages = 3,
                Stories = new List<BasicStoryViewModel>()
                {
                    new BasicStoryViewModel
                    {
                        Category = GetTestCategoriesListWithCategoryDTOs().FirstOrDefault(),
                        Title = "Good",
                        CollectedAmount = 10,
                        User = GetTestUserDTO()
                    }
                },
                SearchWord = string.Empty,
                CurrentUrl = "/Home/Index"
            };
        }

        public static List<StoryDTO> GetTestListWithStoryDtos()
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
                    Title = "Good",
                    User = GetTestUserDTO()
                },
                new StoryDTO
                {
                    Category = new CategoryDTO()
                    {
                        Name = "Sport"
                    },
                    CollectedAmount = 0,
                    CreatedOn = DateTime.UtcNow,
                    Description = "Some description",
                    Title = "Sport to the people",
                    User = GetTestUserDTO()
                },
                new StoryDTO
                {
                    Category = new CategoryDTO()
                    {
                        Name = "Sport"
                    },
                    CollectedAmount = 0,
                    CreatedOn = DateTime.UtcNow,
                    Description = "Some description",
                    Title = "Sport to the people",
                    User = GetTestUserDTO()
                }
            };
        }

        public static UserDTO GetTestUserDTO()
        {
            return new UserDTO()
            {
                Email = "vicky.penkova@gmial.com",
                FirstName = "Viki",
                LastName = "Penkova",
                AvilableMoneyAmount = 100
            };
        }

        public static List<CategoryDTO> GetTestCategoriesListWithCategoryDTOs()
        {
            return new List<CategoryDTO>()
            {
               new CategoryDTO()
               {
                   Id = Guid.NewGuid(),
                   Name = "Health"
               },
               new CategoryDTO()
               {
                   Id = Guid.NewGuid(),
                   Name = "Volunteer"
               },
                new CategoryDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Sports"
                }
            };
        }
    }
}
