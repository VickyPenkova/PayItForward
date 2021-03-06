﻿namespace PayItForward.UnitTests.Web.Controllers.StoriesController
{
    using System;
    using System.Security.Claims;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Services.Abstraction;
    using PayItForward.UnitTests.Web.Controllers.Stubs;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class Add_Should
    {
        private readonly PayItForward.Web.Controllers.StoriesController storiesController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<IUsersService> usersService;
        private readonly Mock<IDonationsService> donationService;
        private readonly Mock<ICategoriesService> categoriesService;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IHttpContextAccessor> httpaccessor;
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public Add_Should()
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
        public void RedirectToCurrentUserStories()
        {
            // Arrange
            this.categoriesService.Setup(x => x.GetCategories())
                .Returns(StoriesController_Stubs.GetTestListWithCategoryDTOs());
            this.storiesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, "username")
                        },
                        "someAuthTypeName"))
                }
            };
            this.storiesService.Setup(x => x.Add(new Models.StoryDTO(), "username"))
                .Returns(this.storyId);
            this.httpaccessor.Setup(a => a.HttpContext.User)
                .Returns(this.storiesController.ControllerContext.HttpContext.User);

            // Act
            var result = this.storiesController.Add(this.GetTestAddStoryViewModel());

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("CurrentUserStories", redirectToActionResult.ActionName);
        }

        [Fact]

        public void ReturnAddStoryViewModel_WithCategoryNameSetToNull()
        {
            // Arrange
            var expectedViewModel = new AddStoryViewModel();

            // Act
            var result = this.storiesController.Add();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = (AddStoryViewModel)viewResult.ViewData.Model;

            Assert.Null(actualViewModel.CategoryName);
        }

        private AddStoryViewModel GetTestAddStoryViewModel()
        {
            return new AddStoryViewModel()
            {
                CategoryName = "Healt",
                Description = "albaba",
                GoalAmount = 122,
                Title = "Good"
            };
        }
    }
}
