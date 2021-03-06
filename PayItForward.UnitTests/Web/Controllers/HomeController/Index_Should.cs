﻿namespace PayItForward.UnitTests.Web.Controllers.HomeController
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PayItForward.Common;
    using PayItForward.Services.Abstraction;
    using PayItForward.UnitTests.Web.Controllers.Stubs;
    using PayItForward.Web.Models.CategoryViewModels;
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;
    using Xunit;

    public class Index_Should
    {
        private readonly PayItForward.Web.Controllers.HomeController homeController;
        private readonly Mock<IStoriesService> storiesService;
        private readonly Mock<ICategoriesService> categoriesService;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IHttpContextAccessor> httpaccessor;
        private string categoryname = string.Empty;
        private string subTitle = string.Empty;

        public Index_Should()
        {
            this.mapper = new Mock<IMapper>();
            this.storiesService = new Mock<IStoriesService>();
            this.categoriesService = new Mock<ICategoriesService>();
            this.httpaccessor = new Mock<IHttpContextAccessor>();
            this.homeController = new PayItForward.Web.Controllers.HomeController(
                this.storiesService.Object, this.categoriesService.Object, this.mapper.Object, this.httpaccessor.Object);
        }

        [Fact]
        public void ReturnIndexViewModel_WithAListOfBasicStoryViewModel()
        {
            // Arrange
            this.ArrangeStoriestServiceSetup(this.storiesService);

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void ReturnIndexViewModel_WithCurrentPageProperty_SetToOne()
        {
            // Arrange
            this.ArrangeStoriestServiceSetup(this.storiesService);

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            Assert.Equal(1, indexViewModel.CurrentPage);
        }

        [Fact]
        public void ReturnIndexViewModel_WithListCategoriesViewModel_AsTypeOfCategoriesProperty()
        {
            // Arrange
            this.ArrangeStoriestServiceSetup(this.storiesService);

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs());

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            Assert.IsAssignableFrom<PayItForward.Web.Models.CategoryViewModels.ListCategoriesViewModel[]>(indexViewModel.Categories);
        }

        [Fact]
        public void ReturnIndexViewModel_WithCorrectCategoryNames()
        {
            // Arrange
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtos();
            var dtoCategories = HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs();
            var expectedCategoriesViewModels = dtoCategories.Select(x => new ListCategoriesViewModel()
            {
                Name = x.Name
            }).ToList();

            this.ArrangeStoriestServiceSetup(this.storiesService);

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(dtoCategories);

            this.mapper.Setup(m => m.Map<IEnumerable<ListCategoriesViewModel>>(dtoCategories))
                .Returns(expectedCategoriesViewModels);

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            var actualCategories = indexViewModel.Categories as List<ListCategoriesViewModel>;
            for (int i = 0; i < actualCategories.Count; i++)
            {
                var expectedStory = expectedCategoriesViewModels[i];
                var actualStory = actualCategories[i];

                Assert.Equal(expectedStory.Name, actualStory.Name);
            }
        }

        [Fact]
        public void ReturnCorrectIndexViewModel_WhenRouteParameterCategoryName_IsSet()
        {
            // Arrange
            this.categoryname = GlobalConstants.CategoryHealth;
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtos();
            var dtoCategories = HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs();

            var expectedStoriesViewModels = dtoStories
                .Select(x => new BasicStoryViewModel()
            {
                Category = x.Category
            }).Where(category => category.Category.Name == this.categoryname)
            .ToList();

            this.storiesService.Setup(
                services =>
                        services.CountStories(this.subTitle, this.categoryname))
                        .Returns(dtoStories.Count());

            this.storiesService.Setup(s => s.Stories(3, 0, this.subTitle, this.categoryname))
                .Returns(dtoStories);

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(dtoCategories);

            this.mapper.Setup(m => m.Map<IEnumerable<BasicStoryViewModel>>(dtoStories))
               .Returns(expectedStoriesViewModels);

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            var actualStoriesViewModels = indexViewModel.Stories as List<BasicStoryViewModel>;

            Assert.True(expectedStoriesViewModels.Equals(actualStoriesViewModels));
        }

        [Fact]
        public void ReturnIndexViewModel_WithCorrectStoriesTitles()
        {
            // Arrange
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtos();
            var expectedStoriesViewModels = dtoStories.Select(x => new BasicStoryViewModel()
            {
                Title = x.Title
            }).ToList();

            this.storiesService.Setup(
                services =>
                        services.CountStories(this.subTitle, this.categoryname))
                        .Returns(dtoStories.Count());

            this.storiesService.Setup(s => s.Stories(3, 0, this.subTitle, this.categoryname))
                .Returns(dtoStories);

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs());

            this.mapper.Setup(m => m.Map<IEnumerable<BasicStoryViewModel>>(dtoStories))
                .Returns(expectedStoriesViewModels);

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            var actualStories = indexViewModel.Stories as List<BasicStoryViewModel>;
            for (int i = 0; i < actualStories.Count; i++)
            {
                var expectedStory = expectedStoriesViewModels[i];
                var actualStory = actualStories[i];

                Assert.Equal(expectedStory.Title, actualStory.Title);
            }
        }

        [Fact]
        public void ReturnIndexViewModel_WithCorrectStoriesOwner()
        {
            // Arrange
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtos();
            var expectedStoriesViewModels = dtoStories.Select(x => new BasicStoryViewModel()
            {
                User = x.User
            }).ToList();

            this.storiesService.Setup(s => s.Stories(3, 0, this.subTitle, this.categoryname))
                .Returns(dtoStories);

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs());

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs());

            this.mapper.Setup(m => m.Map<IEnumerable<BasicStoryViewModel>>(dtoStories))
                .Returns(expectedStoriesViewModels);

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            var actualStories = indexViewModel.Stories as List<BasicStoryViewModel>;
            for (int i = 0; i < actualStories.Count; i++)
            {
                var expectedStory = expectedStoriesViewModels[i];
                var actualStory = actualStories[i];

                Assert.Equal(expectedStory.User.Email, actualStory.User.Email);
            }
        }

        [Fact]
        public void ReturnIndexViewModel_WithCorrectNumberOfStories()
        {
            // Arrange
            var dtoStories = HomeController_Stubs.GetTestListWithStoryDtos();
            var expectedStoriesViewModels = dtoStories.Select(x => new BasicStoryViewModel()
            {
                Title = x.Title,
                Category = x.Category
            }).ToList();

            this.storiesService.Setup(
                services =>
                        services.CountStories(this.subTitle, this.categoryname))
                        .Returns(dtoStories.Count());

            this.storiesService.Setup(s => s.Stories(3, 0, this.subTitle, this.categoryname))
                .Returns(dtoStories);

            this.categoriesService.Setup(category => category.GetCategories())
                .Returns(HomeController_Stubs.GetTestCategoriesListWithCategoryDTOs());

            this.mapper.Setup(m => m.Map<IEnumerable<BasicStoryViewModel>>(dtoStories))
                .Returns(expectedStoriesViewModels);

            // Act
            var result = this.homeController.Index(1, this.categoryname, this.subTitle);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var indexViewModel = (IndexViewModel)viewResult.ViewData.Model;
            var actualStories = indexViewModel.Stories as List<BasicStoryViewModel>;

            Assert.Equal(expectedStoriesViewModels.Count, actualStories.Count);
        }

        private void ArrangeStoriestServiceSetup(Mock<IStoriesService> storiesService)
        {
            this.storiesService.Setup(
                services =>
                        services.CountStories(this.subTitle, this.categoryname))
                        .Returns(HomeController_Stubs.GetTestIndexViewModel().Stories.Count());

            this.storiesService.Setup(s => s.Stories(3, 0, this.subTitle, this.categoryname))
                .Returns(HomeController_Stubs.GetTestListWithStoryDtos());
        }
    }
}
