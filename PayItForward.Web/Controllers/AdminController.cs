namespace PayItForward.Web.Controllers
{
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
        private readonly IUsersService usersService;
        private readonly IStoriesService storiesService;
        private readonly IMapper mapper;

        public AdminController(
            IUsersService usersService,
            IStoriesService storiesService,
            IMapper mapper)
        {
            this.usersService = usersService;
            this.storiesService = storiesService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult UserDetails(string id)
        {
            var userFromDb = this.usersService.GetUserById(id);
            var storiesFromDb = this.storiesService.Stories().Where(s => s.User.Id == id).ToList();
            var donationsFromDb = this.usersService.GetDonations(id);

            if (userFromDb == null)
            {
                return this.Content("User unavailable.");
            }

            var detailedStoryInfo = new List<DetailedStoryViewModel>();

            foreach (var story in storiesFromDb)
            {
                detailedStoryInfo.Add(new DetailedStoryViewModel()
                {
                    CollectedAmount = story.CollectedAmount,
                    Description = story.Description,
                    DateCreated = story.CreatedOn,
                    GoalAmount = story.GoalAmount,
                    Id = story.Id,
                    ExpirationDate = story.ExpirationDate,
                    Title = story.Title,
                    User = userFromDb
                });
            }

            var model = new UserDetailsViewModel()
            {
                Donations = donationsFromDb,
                Stories = detailedStoryInfo,
                User = userFromDb
            };

            return this.View(model);
        }

        public IActionResult DeleteUser(string id)
        {
            var res = this.usersService.Delete(id);
            return this.View();
        }
    }
}