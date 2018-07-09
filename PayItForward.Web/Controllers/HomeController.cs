namespace PayItForward.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models;
    using PayItForward.Web.Models.CategoryViewModels;
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private const int ItemsPerPage = 3;

        private readonly IStoriesService storiesService;
        private readonly ICategoriesService categoriesService;
        private readonly IMapper mapper;

        public HomeController(IStoriesService storiesService, ICategoriesService categoriesService, IMapper mapper)
        {
            this.storiesService = storiesService;
            this.categoriesService = categoriesService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(int id, string categoryName, string search = "")
        {
            var page = id == 0 ? 1 : id;
            int totalNumberOfStories = this.storiesService.CountStories(search, categoryName);
            var totalPages = (int)Math.Ceiling(totalNumberOfStories / (decimal)ItemsPerPage);
            var skip = (page - 1) * ItemsPerPage;
            var stories = this.storiesService.Stories(ItemsPerPage, skip, search, categoryName);
            var categories = this.categoriesService.GetCategories();

            var resultModel = new IndexViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Stories = this.mapper.Map<IEnumerable<BasicStoryViewModel>>(stories),
                SearchWord = search,
                Categories = this.mapper.Map<IEnumerable<ListCategoriesViewModel>>(categories)
            };

            return this.View(resultModel);
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
