namespace PayItForward.UnitTests.Web.Controllers.HomeController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.UnitTests.Web.Controllers.Stubs;
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class MyStories_Should
    {
        private readonly PayItForward.Web.Controllers.HomeController homeController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<ICategoriesService> categoriesService;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IHttpContextAccessor> httpaccessor;

        public MyStories_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.categoriesService = new Mock<ICategoriesService>();
            this.httpaccessor = new Mock<IHttpContextAccessor>();
            this.homeController = new PayItForward.Web.Controllers.HomeController(
                this.storiesService.Object, this.categoriesService.Object, this.mapper.Object, this.httpaccessor.Object);
        }

        [Fact]
        public void ReturnMyStoriesViewModel_WithAListOfBasicStoryViewModel()
        {
            // Arrange
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtos();
            this.homeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, HomeController_Stubs.GetTestUserDTO().Id)
                        },
                        "someAuthTypeName"))
                }
            };
            this.httpaccessor.Setup(a => a.HttpContext.User).Returns(this.homeController.ControllerContext.HttpContext.User);
            this.storiesService.Setup(x => x.Stories())
                .Returns(dtoStories);

            var expectedMyStoriesViewmodel = new List<BasicStoryViewModel>()
               {
                   new BasicStoryViewModel()
                   {
                       User = HomeController_Stubs.GetTestUserDTO()
                   }
               };
            this.mapper.Setup(m => m.Map<List<BasicStoryViewModel>>(dtoStories))
               .Returns(expectedMyStoriesViewmodel);

            // Act
            var result = this.homeController.MyStories();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (MyStoriesViewModel)viewResult.ViewData.Model;
            var actualMyStoriesViewModels = indexViewModel.MyStories as List<BasicStoryViewModel>;

            Assert.True(expectedMyStoriesViewmodel.Equals(actualMyStoriesViewModels));
        }

        [Fact]
        public void ReturnMyStoriesViewModel_WithMessage()
        {
            // Arrange
            this.homeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, HomeController_Stubs.GetTestUserDTO().Id)
                        },
                        "someAuthTypeName"))
                }
            };
            this.httpaccessor.Setup(a => a.HttpContext.User).Returns(this.homeController.ControllerContext.HttpContext.User);
            this.storiesService.Setup(x => x.Stories())
                .Returns(new List<StoryDTO>());

            this.mapper.Setup(m => m.Map<List<BasicStoryViewModel>>(new List<StoryDTO>()))
                .Returns(new List<BasicStoryViewModel>());

            var expectedMyStoriesViewModel = new MyStoriesViewModel()
            {
                Message = "No stories found!"
            };

            // Act
            var result = this.homeController.MyStories();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var storiesViewModel = (MyStoriesViewModel)viewResult.ViewData.Model;
            var actualMyStoriesViewModel = storiesViewModel.Message;

            Assert.Equal(expectedMyStoriesViewModel.Message, actualMyStoriesViewModel);
        }
    }
}
