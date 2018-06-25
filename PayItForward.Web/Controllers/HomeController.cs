namespace PayItForward.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models;
    using PayItForward.Web.Models.DonationViewModels;
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
                Stories = this.mapper.Map<IEnumerable<BasicStoryViewModel>>(stories),
                CurrentUrl = url.ToString(),
                SearchWord = searchString
            };

            return this.View(resultModel);
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {
            var storyFromDb = this.storiesService.GetStories().Where(s => s.Id == id).FirstOrDefault();
            var story = this.mapper.Map<DetailsViewModel>(storyFromDb);

            return this.View(story);
        }

        public IActionResult Donate(Guid id)
        {
            var storyFromDb = this.storiesService.GetStories().Where(s => s.Id == id).FirstOrDefault();
            var storyToDonate = this.mapper.Map<DonateViewModel>(storyFromDb);

            return this.View(storyToDonate);
        }

        [HttpPost]
        public IActionResult MakeDonation(Guid id)
        {
            string moneyInput = this.Request.Form["moneyToDonate"];
            var storyFromDb = this.storiesService.GetStories().Where(s => s.Id == id).FirstOrDefault();

            decimal userAvailableMoney = storyFromDb.User.AvilableMoneyAmount;
            decimal earnedMoney = storyFromDb.CollectedAmount;
            decimal moneyToDonate;

            try
            {
                moneyToDonate = decimal.Parse(moneyInput);
            }
            catch (Exception e)
            {
                return this.View(e.Data);
            }

            var resultModel = new DonateViewModel();

            if (userAvailableMoney >= moneyToDonate
                && moneyToDonate != 0)
            {
                userAvailableMoney -= moneyToDonate;
                earnedMoney += moneyToDonate;

                resultModel = this.mapper.Map<DonateViewModel>(storyFromDb);
                resultModel.CollectedAmount += earnedMoney;
                resultModel.Donations.Amount += earnedMoney;
                this.TempData["message"] = "You have just made a donation!";
            }
            else
            {
                this.TempData["message"] = "Not enough availability!";
            }

            // return this.Redirect(this.ControllerContext.HttpContext.Request.Headers["Referer"].ToString());
            return this.View("Donate", resultModel);
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return this.View("Donate");
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
