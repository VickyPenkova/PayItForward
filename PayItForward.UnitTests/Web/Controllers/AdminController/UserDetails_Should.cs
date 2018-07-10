namespace PayItForward.UnitTests.Web.Controllers.AdminController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.UnitTests.Web.Controllers.Stubs;
    using PayItForward.Web.Models.AdminViewModels;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class UserDetails_Should
    {
        private readonly Mock<IUsersService> usersService;
        private readonly Mock<IStoriesService> storiesServices;
        private readonly Mock<IMapper> mapper;
        private readonly PayItForward.Web.Controllers.AdminController adminController;
        private readonly string userId = "Testuserid@123";
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public UserDetails_Should()
        {
            this.storiesServices = new Mock<IStoriesService>();
            this.usersService = new Mock<IUsersService>();
            this.mapper = new Mock<IMapper>();
            this.adminController = new PayItForward.Web.Controllers.AdminController(
                this.usersService.Object,
                this.storiesServices.Object,
                this.mapper.Object);
        }

        [Fact]
        public void ReturnCorrectUserDetailsViewModel()
        {
            // Arrange
            var userDto = AdminController_Stubs.GetTestUserDto(this.userId);

            var storiesDtos = AdminController_Stubs.GetTestListWIthStoryDtos(this.storyId, this.userId);

            var donationDtos = AdminController_Stubs.GetTestListWithDOnationDtos(this.storyId, this.userId);

            this.usersService.Setup(user => user.GetUserById(this.userId))
                .Returns(userDto);
            this.storiesServices.Setup(s => s.Stories())
                .Returns(storiesDtos.Where(story => story.User.Id == this.userId));
            this.usersService.Setup(u => u.GetDonations(this.userId))
                .Returns(donationDtos);

            // Act
            var result = this.adminController.UserDetails(this.userId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<UserDetailsViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void ReturnUserDatailsModel_WithCorrectUser()
        {
            // Arrange
            var userDto = AdminController_Stubs.GetTestUserDto(this.userId);

            var storiesDtos = AdminController_Stubs.GetTestListWIthStoryDtos(this.storyId, this.userId);

            var donationDtos = AdminController_Stubs.GetTestListWithDOnationDtos(this.storyId, this.userId);

            this.usersService.Setup(user => user.GetUserById(this.userId))
                .Returns(userDto);
            this.storiesServices.Setup(s => s.Stories())
                .Returns(storiesDtos.Where(story => story.User.Id == this.userId));
            this.usersService.Setup(u => u.GetDonations(this.userId))
                .Returns(donationDtos);

            // Act
            var result = this.adminController.UserDetails(this.userId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var userDetailsViewModel = (UserDetailsViewModel)viewResult.ViewData.Model;

            Assert.Equal(userDto, userDetailsViewModel.User);
        }

        [Fact]
        public void ReturnContent_UserUnavailable_WhenUserIsNull()
        {
            // Arrange
            var storiesDtos = AdminController_Stubs.GetTestListWIthStoryDtos(this.storyId, this.userId);

            var donationDtos = AdminController_Stubs.GetTestListWithDOnationDtos(this.storyId, this.userId);

            this.usersService.Setup(user => user.GetUserById(this.userId))
                .Returns((UserDTO)null);
            this.storiesServices.Setup(s => s.Stories())
                .Returns(storiesDtos.Where(story => story.User.Id == this.userId));
            this.usersService.Setup(u => u.GetDonations(this.userId))
                .Returns(donationDtos);

            // Act
            var result = this.adminController.UserDetails(this.userId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("User unavailable.", contentResult.Content);
        }
    }
}
