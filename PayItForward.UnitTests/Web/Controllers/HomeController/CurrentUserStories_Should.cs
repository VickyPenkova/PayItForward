namespace PayItForward.UnitTests.Web.Controllers.HomeController
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.UnitTests.Web.Controllers.Stubs;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class CurrentUserStories_Should
    {
        private readonly PayItForward.Web.Controllers.HomeController homeController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<ICategoriesService> categoriesService;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IHttpContextAccessor> httpaccessor;
        private readonly UserDTO currentUser = HomeController_Stubs.GetTestUserDTO();

        public CurrentUserStories_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.categoriesService = new Mock<ICategoriesService>();
            this.httpaccessor = new Mock<IHttpContextAccessor>();
            this.homeController = new PayItForward.Web.Controllers.HomeController(
                this.storiesService.Object, this.categoriesService.Object, this.mapper.Object, this.httpaccessor.Object);

            this.homeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, this.currentUser.Id)
                        },
                        "someAuthTypeName"))
                }
            };
        }

        [Fact]
        public void ReturnCurrentUserStoriesViewModel_WithAListOfBasicStoryViewModel()
        {
            // Arrange
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtosWithSameUser();
            this.httpaccessor.Setup(a => a.HttpContext.User).Returns(this.homeController.ControllerContext.HttpContext.User);
            this.storiesService.Setup(x => x.Stories())
                .Returns(dtoStories);

            var expectedViewModel = new List<BasicStoryViewModel>()
               {
                   new BasicStoryViewModel()
                   {
                       User = this.currentUser
                   }
               };
            this.mapper.Setup(m => m.Map<List<BasicStoryViewModel>>(dtoStories))
               .Returns(expectedViewModel);

            // Act
            var result = this.homeController.CurrentUserStories();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualViewModel = (List<BasicStoryViewModel>)viewResult.ViewData.Model;

            Assert.True(expectedViewModel.Equals(actualViewModel));
        }

        [Fact]
        public void ReturnNotTheSameNumberOfCurrentUserStoriesViewModel_WhenTheyAreTwoUsers()
        {
            // Arrange
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtosWithSameUser();

            this.httpaccessor.Setup(a => a.HttpContext.User)
                .Returns(this.homeController.ControllerContext.HttpContext.User);

            this.storiesService.Setup(x => x.Stories())
                .Returns(dtoStories);

            var expectedMyStoriesViewmodel = new List<BasicStoryViewModel>()
            {
                new BasicStoryViewModel()
                {
                    User = this.GetTestUser()
                },
                new BasicStoryViewModel()
                {
                    User = this.currentUser
                },
                new BasicStoryViewModel()
                {
                    User = this.currentUser
                },
            };

            this.mapper.Setup(m => m.Map<List<BasicStoryViewModel>>(dtoStories))
                .Returns(expectedMyStoriesViewmodel);

            // Act
            var result = this.homeController.CurrentUserStories();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var actualMyStoriesViewModels = (List<BasicStoryViewModel>)viewResult.ViewData.Model;

            Assert.NotEqual(
                expectedMyStoriesViewmodel
                .FindAll(x => x.User.Id == "@id1")
                .Count(),
                actualMyStoriesViewModels
                .Count());
        }

        private UserDTO GetTestUser()
        {
            return new UserDTO()
            {
                FirstName = "Fo",
                LastName = "Fi",
                Id = "@id1"
            };
        }
    }
}
