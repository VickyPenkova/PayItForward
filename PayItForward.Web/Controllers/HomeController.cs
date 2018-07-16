namespace PayItForward.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Common;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models;
    using PayItForward.Web.Models.CategoryViewModels;
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;

    public class HomeController : Controller
    {
        private const int ItemsPerPage = 3;

        private readonly IStoriesService storiesService;
        private readonly ICategoriesService categoriesService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpaccessor;

        public HomeController(
            IStoriesService storiesService,
            ICategoriesService categoriesService,
            IMapper mapper,
            IHttpContextAccessor httpaccessor)
        {
            this.storiesService = storiesService;
            this.categoriesService = categoriesService;
            this.mapper = mapper;
            this.httpaccessor = httpaccessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(int id, string categoryName, string search = "")
        {
            var page = id < 1 ? 1 : id;
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

        [Authorize]
        public IActionResult MyStories()
        {
            var stories = this.storiesService.Stories()
                .Where(story => story.User.Id == this.httpaccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ToList();

            var resultModel = new MyStoriesViewModel()
            {
                MyStories = this.mapper.Map<List<BasicStoryViewModel>>(stories)
            };

            if (resultModel.MyStories.Count() < 1)
            {
                resultModel = new MyStoriesViewModel()
                {
                    MyStories = this.mapper.Map<List<BasicStoryViewModel>>(stories),
                    Message = "No stories found!"
                };

                return this.View(resultModel);
            }

            return this.View(resultModel);
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
