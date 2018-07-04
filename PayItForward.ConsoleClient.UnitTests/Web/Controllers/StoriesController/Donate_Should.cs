namespace PayItForward.UnitTests.Web.Controllers.StoriesController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.DonationViewModels;
    using Xunit;

    public class Donate_Should
    {
        private readonly PayItForward.Web.Controllers.StoriesController storiesController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<IUsersService> usersService;
        private readonly Mock<IDonationsService> donationService;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IHttpContextAccessor> httpaccessor;
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public Donate_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.donationService = new Mock<IDonationsService>();
            this.usersService = new Mock<IUsersService>();
            this.httpaccessor = new Mock<IHttpContextAccessor>();
            this.storiesController = new PayItForward.Web.Controllers.StoriesController(
                this.storiesService.Object,
                this.donationService.Object,
                this.usersService.Object,
                this.mapper.Object,
                this.httpaccessor.Object);
        }

        [Fact]
        public void ReturnARedirectToIndexHomeWhenIdIsEmpty()
        {
            // Arrange

            // Act
            var result = this.storiesController.Donate(id: Guid.Empty);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Stories", redirectToActionResult.ControllerName);
            Assert.Equal("Details", redirectToActionResult.ActionName);
        }

        [Fact]
        public void ReturnContent_WithStoryNotFound_WhenStoryNotFound()
        {
            // Arrange
            this.storiesService.Setup(s => s.GetStoryById(this.storyId))
                .Returns((StoryDTO)null);

            // Act
            var result = this.storiesController.Donate(this.storyId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Story not found.", contentResult.Content);
        }

        [Fact]

        public void ReturnResultContent_WithNoUserToDonate_WhenDonatorIdIsNullOrEmpty()
        {
            // Arrange
            this.storiesService.Setup(s => s.GetStoryById(this.storyId))
                .Returns(this.HelperStoryDto().FirstOrDefault());
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
            this.usersService.Setup(u => u.GetUserById(" ")).Returns((UserDTO)null);

            // Act
            var result = this.storiesController.Donate(this.HelperDonateViewModel(), this.storyId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("No user to donate.", contentResult.Content);
        }

        [Fact]
        public void ReturnContentResult_WithStoryNotFound_WhenGivenInvalidModel()
        {
            // Arrange & Act
            var result = this.storiesController.Donate(model: null, id: this.storyId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Story not found.", contentResult.Content);
        }

        [Fact]
        public void ReturnContentResult_WithStoryNotFound_WhenIdIsNull()
        {
            // Arrange & Act
            var result = this.storiesController.Donate(model: new DonateViewModel(), id: Guid.Empty);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Story not found.", contentResult.Content);
        }

        [Fact]
        public void CallGetUserByIdOnce()
        {
            // Arrange
            this.usersService.Setup(story => story.GetUserById("username"))
                .Returns(this.HelperUserDTO());

            this.storiesService.Setup(s => s.GetStoryById(this.storyId))
                .Returns(this.HelperStoryDto().FirstOrDefault());

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

            // Act
            var result = this.storiesController.Donate(
                new DonateViewModel()
                {
                    Amount = 10,
                    Donator = new UserDTO(),
                    CollectedAmount = 900,
                    Title = "help"
                }, this.storyId);

            // Assert
            this.usersService.Verify(m => m.GetUserById("username"), Times.Once);
        }

        [Fact]
        public void ReturnViewResult_WithDonateViewModel()
        {
            // Arrange
            this.usersService.Setup(story => story.GetUserById("username"))
                .Returns(this.HelperUserDTO());

            this.storiesService.Setup(s => s.GetStoryById(this.storyId))
                .Returns(this.HelperStoryDto().FirstOrDefault());

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
            var donationDTO = new DonationDTO()
            {
                Amount = 10,
                Donator = new UserDTO(),
                Story = this.HelperStoryDto().FirstOrDefault()
            };

            this.donationService.Setup(d => d.Add(donationDTO, this.storyId)).Returns(true);

            // Act
            var result = this.storiesController.Donate(
                new DonateViewModel()
                {
                    Amount = 10,
                    Donator = new UserDTO(),
                    CollectedAmount = 900,
                    Title = "help"
                }, this.storyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<DonateViewModel>(viewResult.ViewData.Model);
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
                    CollectedAmount = 3,
                    CreatedOn = DateTime.UtcNow,
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
                AvilableMoneyAmount = 100,
                Id = "alabala"
            };
        }

        private DonateViewModel HelperDonateViewModel()
        {
            return new DonateViewModel()
            {
                Amount = 2,
                CollectedAmount = 3,
                Donator = this.HelperUserDTO(),
                GoalAmount = 3000,
                Title = "Title"
            };
        }
    }
}
