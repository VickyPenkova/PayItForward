namespace PayItForward.UnitTests.Web.Controllers.DonationController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.DonationViewModels;
    using Xunit;

    public class Donate_Should
    {
        private readonly PayItForward.Web.Controllers.DonationController donationController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<IUsersService> usersService;
        private readonly Mock<IDonationsService> donationService;
        private readonly Mock<IMapper> mapper;
        private Guid storyId = new Guid("E6E635AB-6AD9-40BD-9992-08D5D82FC3F0");

        public Donate_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.donationService = new Mock<IDonationsService>();
            this.usersService = new Mock<IUsersService>();
            this.donationController = new PayItForward.Web.Controllers.DonationController(
                this.donationService.Object,
                this.storiesService.Object,
                this.usersService.Object,
                this.mapper.Object);
        }

        [Fact]
        public void ReturnContent_WithStoryNotFound_WhenStoryNotFound()
        {
            // Arrange
            this.storiesService.Setup(s => s.GetStoryById(this.storyId))
                .Returns((StoryDTO)null);

            // Act
            var result = this.donationController.Donate(this.storyId);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal("Story not found.", contentResult.Content);
        }

        [Fact]
        public void ReturnViewResult_WithDonateViewModel()
        {
            // Arrange
            this.storiesService.Setup(x => x.GetStoryById(this.storyId))
                .Returns(this.HelperStoryDto().FirstOrDefault());

            var result = this.donationController.Donate(this.storyId);

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
