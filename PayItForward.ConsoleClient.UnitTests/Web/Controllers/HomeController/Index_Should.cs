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
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class Index_Should
    {
        private readonly PayItForward.Web.Controllers.HomeController homeController;
        private readonly Mock<IStoriesService> storiesServices;
        private readonly Mock<IMapper> mapper;

        public Index_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesServices = new Mock<IStoriesService>();
            this.homeController = new PayItForward.Web.Controllers.HomeController(this.storiesServices.Object, this.mapper.Object);
        }

        [Fact]
        public void ReturnIndexViewModel_WithAListOfBasicStoryViewModel()
        {
            // Arrange
            var empty = " ";
            var page = 1;
            var totalNumberOfStories = this.storiesServices.Setup(
                services =>
                        services.CountStories(empty))
                        .Returns(this.HelperIndexViewModelToTest().Stories.Count());

            var totalPages = (int)Math.Ceiling(1 / (decimal)3);
            var skip = (page - 1) * 3;
            var stories = this.storiesServices.Setup(s => s.GetStories(3, skip, empty))
                .Returns(this.HelperStoryDto());

            // Act
            var result = this.homeController.Index("/Home/Index/");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void CallCountStoriesOnce()
        {
            // Arrange
            var empty = " ";
            var page = 1;
            var totalNumberOfStories = this.storiesServices.Setup(
                services =>
                        services.CountStories("/Home/Index/"))
                        .Returns(this.HelperIndexViewModelToTest().Stories.Count());
            var totalPages = (int)Math.Ceiling(1 / (decimal)3);
            var skip = (page - 1) * 3;
            var stories = this.storiesServices.Setup(s => s.GetStories(3, skip, empty))
                .Returns(this.HelperStoryDto());

            // Act
            var result = this.homeController.Index("/Home/Index/");

            // Assert
            this.storiesServices.Verify(x => x.CountStories("/Home/Index/"), Times.Once);
        }

        [Fact]
        public void CallGetStoriesOnce()
        {
            // Arrange
            var empty = " ";
            var page = 1;
            var totalNumberOfStories = this.storiesServices.Setup(
                services =>
                        services.CountStories("/Home/Index/"))
                        .Returns(this.HelperIndexViewModelToTest().Stories.Count());
            var totalPages = (int)Math.Ceiling(1 / (decimal)3);
            var skip = (page - 1) * 3;
            var stories = this.storiesServices.Setup(s => s.GetStories(3, skip, empty))
                .Returns(this.HelperStoryDto());

            // Act
            var result = this.homeController.Index("/Home/Index/");

            // Assert
            this.storiesServices.Verify(x => x.GetStories(3, 0, "/Home/Index/"), Times.Once);
        }

        private IndexViewModel HelperIndexViewModelToTest()
        {
            var basicStoryViewModel = this.HelperStoryDto();
            return new IndexViewModel
            {
                CurrentPage = 1,
                TotalPages = 3,
                Stories = this.mapper.Object.Map<IEnumerable<BasicStoryViewModel>>(basicStoryViewModel),
                SearchWord = string.Empty,
                CurrentUrl = "/Home/Index"
            };
        }

        private List<StoryDTO> HelperStoryDto()
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
                    DateCreated = DateTime.UtcNow,
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
    }
}
