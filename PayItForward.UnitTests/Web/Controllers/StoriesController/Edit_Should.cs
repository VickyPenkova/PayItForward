namespace PayItForward.UnitTests.Web.Controllers.StoriesController
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class Edit_Should
    {
        private readonly PayItForward.Web.Controllers.StoriesController storiesController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<IUsersService> usersService;
        private readonly Mock<IDonationsService> donationService;
        private readonly Mock<ICategoriesService> categoriesService;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IHttpContextAccessor> httpaccessor;
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public Edit_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.donationService = new Mock<IDonationsService>();
            this.usersService = new Mock<IUsersService>();
            this.httpaccessor = new Mock<IHttpContextAccessor>();
            this.categoriesService = new Mock<ICategoriesService>();
            this.storiesController = new PayItForward.Web.Controllers.StoriesController(
                this.storiesService.Object,
                this.donationService.Object,
                this.usersService.Object,
                this.categoriesService.Object,
                this.mapper.Object,
                this.httpaccessor.Object);
        }

        [Fact]
        public void RedirectToCurrentUserStories_WhenIdIsEmpty()
        {
            // Act & Arrange
            var result = this.storiesController.Edit(new EditStoryViewModel(), Guid.Empty);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("CurrentUserStories", redirectToActionResult.ActionName);
        }

        [Fact]
        public void ReturnContent_WithStoryNotFound_WhenStoryNotFound()
        {
            // Arrange
            this.storiesService.Setup(s => s.GetStoryById(this.storyId))
                .Returns((StoryDTO)null);

            // Act
            var result = this.storiesController.Edit(new EditStoryViewModel(), this.storyId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Story not found.", contentResult.Content);
        }

        [Fact]
        public void ReturnToCurrentUserStories()
        {
            // Arrange
            this.storiesService.Setup(x => x.GetStoryById(this.storyId)).Returns(Stubs.StoriesController_Stubs.GetTestListOfStoryDtos().FirstOrDefault());

            // Act
            var result = this.storiesController.Edit(new EditStoryViewModel(), this.storyId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("CurrentUserStories", redirectToActionResult.ActionName);
        }

        private EditStoryViewModel GetTestEditStoryViewModel()
        {
            return new EditStoryViewModel()
            {
                Description = "Test description",
                ExpirationDate = DateTime.Now,
                GoalAmount = 3444,
                Id = this.storyId,
                Title = "test"
            };
        }
    }
}
