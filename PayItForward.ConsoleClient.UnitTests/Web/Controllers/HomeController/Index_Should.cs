namespace PayItForward.UnitTests.Web.Controllers.HomeController
{
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Services.Abstraction;
    using PayItForward.UnitTests.Web.Controllers.Stubs;
    using PayItForward.Web.Models.CategoryViewModels;
    using PayItForward.Web.Models.HomeViewModels;
    using Xunit;

    public class Index_Should
    {
        private readonly PayItForward.Web.Controllers.HomeController homeController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<ICategoriesService> categoriesService;
        private readonly Mock<IMapper> mapper;

        public Index_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.categoriesService = new Mock<ICategoriesService>();
            this.homeController = new PayItForward.Web.Controllers.HomeController(
                this.storiesService.Object, this.categoriesService.Object, this.mapper.Object);
        }

        [Fact]
        public void ReturnIndexViewModel_WithAListOfBasicStoryViewModel()
        {
            // Arrange
            var categoryname = string.Empty;
            var subTitle = string.Empty;
            this.storiesService.Setup(
                services =>
                        services.CountStories(subTitle, categoryname))
                        .Returns(HomeController_Stubs.GetTestIndexViewModel().Stories.Count());

            var stories = this.storiesService.Setup(s => s.Stories(3, 0, subTitle, categoryname))
                .Returns(HomeController_Stubs.GetTestListOfStoryDtos());

            // Act
            var result = this.homeController.Index(1, categoryname, "/Home/Index/");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void ReturnIndexViewModel_WithCurrentPagePropertySetToOne()
        {
            // Arrange
            var categoryname = string.Empty;
            var subTitle = string.Empty;
            this.storiesService.Setup(
                services =>
                        services.CountStories(subTitle, categoryname))
                        .Returns(HomeController_Stubs.GetTestIndexViewModel().Stories.Count());

            var stories = this.storiesService.Setup(s => s.Stories(3, 0, subTitle, categoryname))
                .Returns(HomeController_Stubs.GetTestListOfStoryDtos());

            // Act
            var result = this.homeController.Index(1, categoryname, "/Home/Index/");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            Assert.Equal(1, indexViewModel.CurrentPage);
        }

        [Fact]
        public void ReturnIndexViewModel_WithListCategoriesViewModelAsTypeOfCategoriesProperty()
        {
            // Arrange
            var categoryname = string.Empty;
            var subTitle = string.Empty;
            this.storiesService.Setup(
                services =>
                        services.CountStories(subTitle, categoryname))
                        .Returns(HomeController_Stubs.GetTestIndexViewModel().Stories.Count());

            this.storiesService.Setup(s => s.Stories(3, 0, subTitle, categoryname))
                .Returns(HomeController_Stubs.GetTestListOfStoryDtos());

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs());

            // Act
            var result = this.homeController.Index(1, categoryname, "/Home/Index/");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            Assert.IsAssignableFrom<PayItForward.Web.Models.CategoryViewModels.ListCategoriesViewModel[]>(indexViewModel.Categories);
        }
    }
}
