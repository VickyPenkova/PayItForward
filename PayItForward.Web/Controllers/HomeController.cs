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
    using PayItForward.Web.Models.HomeViewModels;
    using PayItForward.Web.Models.StoryViewModels;

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

        [HttpGet]
        public IActionResult Index(string search = "", int id = 1)
        {
            var page = id;
            int totalNumberOfStories = this.storiesService.CountStories(search);
            var totalPages = (int)Math.Ceiling(totalNumberOfStories / (decimal)ItemsPerPage);
            var skip = (page - 1) * ItemsPerPage;
            var stories = this.storiesService.GetStories(ItemsPerPage, skip, search);

            var resultModel = new IndexViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Stories = this.mapper.Map<IEnumerable<BasicStoryViewModel>>(stories),
                SearchWord = search
            };

            return this.View(resultModel);
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                return this.RedirectToAction(actionName: nameof(this.Index), controllerName: "Home");
            }

            var storyFromDb = this.storiesService.GetStoryById(id);
            if (storyFromDb == null)
            {
                return this.Content("Story not found.");
            }

            var viewModel = new DetailsViewModel()
            {
                CollectedAmount = storyFromDb.CollectedAmount,
                DateCreated = storyFromDb.DateCreated,
                Description = storyFromDb.Description,
                GoalAmount = storyFromDb.GoalAmount,
                Id = storyFromDb.Id,
                Title = storyFromDb.Title,
                User = storyFromDb.User
            };

            return this.View(viewModel);
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
