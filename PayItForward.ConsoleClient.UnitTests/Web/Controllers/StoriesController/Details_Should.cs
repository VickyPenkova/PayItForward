namespace PayItForward.UnitTests.Web.Controllers.HomeController
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.UnitTests.Web.Controllers.Stubs;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    // These unit tests are only testing what the code in the action method does
    public class Details_Should
    {
        private readonly PayItForward.Web.Controllers.StoriesController storiesController;
        private readonly Mock<IStoriesService> storiesServices;
        private readonly Mock<IDonationsService> donationsService;
        private readonly Mock<IUsersService> usersService;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IHttpContextAccessor> httpaccessor;
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public Details_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesServices = new Mock<IStoriesService>();
            this.donationsService = new Mock<IDonationsService>();
            this.usersService = new Mock<IUsersService>();
            this.httpaccessor = new Mock<IHttpContextAccessor>();
            this.storiesController = new PayItForward.Web.Controllers.StoriesController(
                this.storiesServices.Object,
                this.donationsService.Object,
                this.usersService.Object,
                this.mapper.Object,
                this.httpaccessor.Object);
        }

        [Fact]
        public void ReturnNotNullViewResult()
        {
            // Arrange
            this.storiesServices.Setup(x => x.GetStoryById(this.storyId))
               .Returns(StoriesController_Stubs.GetTestStoriesListWithStoryDtos().FirstOrDefault());

            // Act
            var result = this.storiesController.Details(this.storyId) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ReturnARedirectToIndexHomeWhenIdIsEmpty()
        {
            // Arrange

            // Act
            var result = this.storiesController.Details(id: Guid.Empty);

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
            var result = this.storiesController.Details(this.storyId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Story not found.", contentResult.Content);
        }

        [Fact]
        public void ReturnDetailsViewModel_WithCorrectUser()
        {
            // Arrange
            var expectedDtoStories = StoriesController_Stubs.GetTestStoriesListWithStoryDtos();
            var expectedOwnerOfStory = expectedDtoStories.FirstOrDefault().User;
            this.storiesServices.Setup(x => x.GetStoryById(this.storyId))
                .Returns(expectedDtoStories
                    .FirstOrDefault());

            // Act
            var result = this.storiesController.Details(this.storyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var detailedStoryViewModel = (DetailedStoryViewModel)viewResult.ViewData.Model;
            var actualUser = detailedStoryViewModel.User;

            Assert.Equal(expectedOwnerOfStory, actualUser);
        }

        [Fact]
        public void ReturnViewResult_WithDetailsViewModel()
        {
            // Arrange
            this.storiesServices.Setup(x => x.GetStoryById(this.storyId))
                .Returns(StoriesController_Stubs.GetTestStoriesListWithStoryDtos().FirstOrDefault());

            // Act
            var result = this.storiesController.Details(this.storyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<DetailedStoryViewModel>(viewResult.ViewData.Model);
        }
    }
}
