namespace PayItForward.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Services.Data.Abstraction;
    using PayItForward.Web.Models;

    public class HomeController : Controller
    {
        private readonly IStoriesService storiesService;

        public HomeController(IStoriesService storiesService)
        {
            this.storiesService = storiesService;
        }

        public IActionResult Index()
        {
            var stories = this.storiesService.GetStories(7).ToList();
            return this.View(stories);
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
