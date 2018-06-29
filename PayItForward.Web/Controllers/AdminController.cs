namespace PayItForward.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Common;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.AdminViewModels;
    using PayItForward.Web.Models.StoryViewModels;

    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class AdminController : Controller
    {
        private const int ItemsPerPage = 3;

        private readonly IUsersService usersService;
        private readonly IStoriesService storiesService;
        private readonly IDonationsService donationsService;
        private readonly IMapper mapper;

        public AdminController(
            IUsersService usersService,
            IStoriesService storiesService,
            IDonationsService donationsService,
            IMapper mapper)
        {
            this.usersService = usersService;
            this.storiesService = storiesService;
            this.donationsService = donationsService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult UserDetails(string id)
        {
            var userFromDb = this.usersService.GetUserById(id);
            var donationsFromDb = this.donationsService.GetDonationsByUserId(id);
            var storiesFromDb = this.storiesService.GetStories().Where(s => s.User.Id == id);

            if (userFromDb == null)
            {
                return this.Content("User unavailable.");
            }

            var model = new UserDetailsViewModel()
            {
                Donations = donationsFromDb,
                Stories = storiesFromDb,
                User = userFromDb
            };

            return this.View(model);
        }
    }
}