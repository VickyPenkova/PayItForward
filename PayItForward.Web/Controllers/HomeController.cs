namespace PayItForward.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Services.Data.Abstraction;
    using PayItForward.Web.Models;
    using PayItForward.Web.Models.HomeViewModel;

    public class HomeController : Controller
    {
        private const int ItemsPerPage = 3;

        private readonly IStoriesService storiesService;
        private readonly IMapper mapper;

        public HomeController(IStoriesService storiesService, IMapper mapper)
        {
            this.storiesService = storiesService;
            this.mapper = mapper;
        }

        // TODO: Refactor paging
        [HttpGet]
        public IActionResult Index(int id = 1)
        {
            var url = this.Request.Path.ToString();

            var searchString = this.HttpContext.Request.Query["search"].ToString() ?? string.Empty;

            var page = id;
            int totalNumberOfStories = this.storiesService.CountStories(searchString);
            var totalPages = (int)Math.Ceiling(totalNumberOfStories / (decimal)ItemsPerPage);
            var skip = (page - 1) * ItemsPerPage;
            var stories = this.storiesService.GetStories(ItemsPerPage, skip, searchString);

            var resultModel = new IndexViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Stories = stories,
                CurrentUrl = url.ToString(),
                SearchWord = searchString
            };

            return this.View(resultModel);
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return this.View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";

            return this.View();
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
