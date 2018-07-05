namespace PayItForward.UnitTests.Web.Controllers.HomeController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.CategoryViewModels;
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class Index_Should
    {
        private readonly PayItForward.Web.Controllers.HomeController homeController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<ICategoriesService> categoriesService;
        private readonly Mock<IMapper> mapper;

        public Index_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.categoriesService = new Mock<ICategoriesService>();
            this.homeController = new PayItForward.Web.Controllers.HomeController(
                this.storiesService.Object, this.categoriesService.Object, this.mapper.Object);
        }

        [Fact]
        public void ReturnIndexViewModel_WithAListOfBasicStoryViewModel()
        {
            // Arrange
            var categoryname = string.Empty;
            var subTitle = string.Empty;
            this.storiesService.Setup(
                services =>
                        services.CountStories(subTitle, categoryname))
                        .Returns(this.GetTestIndexViewModel().Stories.Count());

            var stories = this.storiesService.Setup(s => s.Stories(3, 0, subTitle, categoryname))
                .Returns(this.GetTestStoryDto());

            // Act
            var result = this.homeController.Index(1, categoryname, "/Home/Index/");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void ReturnIndexViewModel_WithCurrentPagePropertySetToOne()
        {
            // Arrange
            var categoryname = string.Empty;
            var subTitle = string.Empty;
            this.storiesService.Setup(
                services =>
                        services.CountStories(subTitle, categoryname))
                        .Returns(this.GetTestIndexViewModel().Stories.Count());

            var stories = this.storiesService.Setup(s => s.Stories(3, 0, subTitle, categoryname))
                .Returns(this.GetTestStoryDto());

            // Act
            var result = this.homeController.Index(1, categoryname, "/Home/Index/");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            Assert.Equal(1, indexViewModel.CurrentPage);
        }

        [Fact]
        public void ReturnIndexViewModel_WithListCategoriesViewModelAsTypeOfCtaegoriesProperty()
        {
            // Arrange
            var categoryname = string.Empty;
            var subTitle = string.Empty;
            this.storiesService.Setup(
                services =>
                        services.CountStories(subTitle, categoryname))
                        .Returns(this.GetTestIndexViewModel().Stories.Count());

            this.storiesService.Setup(s => s.Stories(3, 0, subTitle, categoryname))
                .Returns(this.GetTestStoryDto());

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(this.GetTestCategoriesListWithCategoryDTOs());

            // Act
            var result = this.homeController.Index(1, categoryname, "/Home/Index/");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            Assert.IsAssignableFrom<PayItForward.Web.Models.CategoryViewModels.ListCategoriesViewModel[]>(indexViewModel.Categories);
        }

        public IndexViewModel GetTestIndexViewModel()
        {
            var basicStoryViewModel = this.GetTestStoryDto();
            return new IndexViewModel
            {
                CurrentPage = 1,
                TotalPages = 3,
                Stories = this.mapper.Object.Map<IEnumerable<BasicStoryViewModel>>(basicStoryViewModel),
                SearchWord = string.Empty,
                CurrentUrl = "/Home/Index",
                Categories = this.mapper.Object.Map<IEnumerable<ListCategoriesViewModel>>(this.GetTestCategoriesListWithCategoryDTOs())
            };
        }

        public List<StoryDTO> GetTestStoryDto()
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
                    User = new UserDTO()
                    {
                        Email = "vicky.penkova@gmial.com",
                        FirstName = "Viki",
                        LastName = "Penkova",
                        AvilableMoneyAmount = 100
                    }
                }
            };
        }

        public List<CategoryDTO> GetTestCategoriesListWithCategoryDTOs()
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
