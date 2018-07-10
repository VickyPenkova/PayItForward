namespace PayItForward.UnitTests.Web.Controllers.AdminController
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Services.Abstraction;
    using Xunit;

    public class DeleteUser_Should
    {
        private readonly Mock<IUsersService> usersService;
        private readonly Mock<IStoriesService> storiesServices;
        private readonly Mock<IMapper> mapper;
        private readonly PayItForward.Web.Controllers.AdminController adminController;
        private readonly string userId = "Testuserid@123";
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public DeleteUser_Should()
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
        public void ReturnARedirectToIndexHome_WhenIdIsEmpty()
        {
            // Arrange

            // Act
            var result = this.adminController.DeleteUser(id: string.Empty);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", redirectToActionResult.ControllerName);
            Assert.Equal("UserDetails", redirectToActionResult.ActionName);
        }

        [Fact]
        public void ReturnNotNullViewResult()
        {
            // Arrange
            this.usersService.Setup(user => user.Delete(this.userId)).Returns(this.userId);

            // Act
            var result = this.adminController.DeleteUser(id: this.userId) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ReturnViewResult()
        {
            // Arange
            this.usersService.Setup(user => user.Delete(this.userId)).Returns(this.userId);

            // Act
            var result = this.adminController.DeleteUser(id: this.userId);

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
