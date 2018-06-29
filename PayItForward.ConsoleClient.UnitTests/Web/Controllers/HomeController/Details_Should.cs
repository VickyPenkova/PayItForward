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
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    // These unit tests are only testing what the code in the action method does
    public class Details_Should
    {
        private readonly PayItForward.Web.Controllers.HomeController homeController;
        private readonly Mock<IStoriesService> storiesServices;
        private readonly Mock<ICategoriesService> categogoriesService;
        private readonly Mock<IMapper> mapper;
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public Details_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesServices = new Mock<IStoriesService>();
            this.categogoriesService = new Mock<ICategoriesService>();
            this.homeController = new PayItForward.Web.Controllers.HomeController(
                this.storiesServices.Object, this.categogoriesService.Object, this.mapper.Object);
        }

        [Fact]
        public void ReturnNotNullViewResult()
        {
            // Arrange
            this.storiesServices.Setup(x => x.GetStoryById(this.storyId))
               .Returns(this.HelperStoryDto().FirstOrDefault());

            // Act
            var result = this.homeController.Details(this.storyId) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ReturnARedirectToIndexHomeWhenIdIsEmpty()
        {
            // Arrange

            // Act
            var result = this.homeController.Details(id: Guid.Empty);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void ReturnContent_WithStoryNotFound_WhenStoryNotFound()
        {
            // Arrange
            var setup = this.storiesServices.Setup(s => s.GetStoryById(this.storyId)).Returns((StoryDTO)null);

            // Act
            var result = this.homeController.Details(this.storyId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Story not found.", contentResult.Content);
        }

        [Fact]
        public void ReturnViewResult_WithDetailsViewModel()
        {
            // Arrange
            this.storiesServices.Setup(x => x.GetStoryById(this.storyId))
                .Returns(this.HelperStoryDto().FirstOrDefault());

            // Act
            var result = this.homeController.Details(this.storyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<DetailedStoryViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void CallGetStoryByIdOnce()
        {
            // Arrange
            this.storiesServices.Setup(story => story.GetStoryById(this.storyId))
                .Returns(this.HelperStoryDto().FirstOrDefault());

            // Act
            var result = this.homeController.Details(this.storyId);

            // Assert
            this.storiesServices.Verify(m => m.GetStoryById(this.storyId), Times.Once);
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
                    User = this.HelperUserDTO(),
                    Id = this.storyId
                }
            };
        }

        private UserDTO HelperUserDTO()
        {
            return new Models.UserDTO()
            {
                Email = "vicky.penkova@gmial.com",
                FirstName = "Viki",
                LastName = "Penkova",
                AvilableMoneyAmount = 100
            };
        }
    }
}
