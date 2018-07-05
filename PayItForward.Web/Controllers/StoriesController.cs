namespace PayItForward.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.DonationViewModels;
    using PayItForward.Web.Models.StoryViewModels;

    public class StoriesController : Controller
    {
        private readonly IStoriesService storiesService;
        private readonly IDonationsService donationsService;
        private readonly IUsersService usersService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpaccessor;

        public StoriesController(
            IStoriesService storiesService,
            IDonationsService donationsService,
            IUsersService usersService,
            IMapper mapper,
            IHttpContextAccessor httpaccessor)
        {
            this.storiesService = storiesService;
            this.donationsService = donationsService;
            this.usersService = usersService;
            this.mapper = mapper;
            this.httpaccessor = httpaccessor;
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                return this.RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            var storyFromDb = this.storiesService.GetStoryById(id);
            if (storyFromDb == null)
            {
                return this.Content("Story not found.");
            }

            var viewModel = new DetailedStoryViewModel
            {
                CollectedAmount = storyFromDb.CollectedAmount,
                DateCreated = storyFromDb.CreatedOn,
                Description = storyFromDb.Description,
                GoalAmount = storyFromDb.GoalAmount,
                Id = storyFromDb.Id,
                Title = storyFromDb.Title,
                User = storyFromDb.User
            };

            return this.View(viewModel);
        }

        public IActionResult Donate(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.RedirectToAction(actionName: "Details", controllerName: "Stories");
            }

            var storyFromDb = this.storiesService.GetStoryById(id);

            if (storyFromDb == null)
            {
                return this.Content("Story not found.");
            }

            UserDTO test = this.usersService.GetUserById(this.httpaccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var viewModel = new DonateViewModel()
            {
                CollectedAmount = storyFromDb.CollectedAmount,
                GoalAmount = storyFromDb.GoalAmount,
                Donator = this.usersService.GetUserById(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Title = storyFromDb.Title,
                ImageUrl = storyFromDb.ImageUrl
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Donate(DonateViewModel model, Guid id, string returnUrl = null)
        {
            var resultModel = new DonateViewModel();
            var storyFromDb = this.storiesService.GetStoryById(id);
            if (storyFromDb == null)
            {
                return this.Content("Story not found.");
            }

            var donator = this.usersService.GetUserById(this.httpaccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (donator == null)
            {
                return this.Content("No user to donate.");
            }

            if (this.ModelState.IsValid)
            {
                var donation = new DonationDTO()
                {
                    Amount = model.Amount,
                    Story = storyFromDb,
                    Donator = donator
                };

                var makeDonation = this.donationsService.Add(donation, id);
                if (makeDonation == true)
                {
                    resultModel = new DonateViewModel()
                    {
                        ImageUrl = storyFromDb.ImageUrl,
                        Amount = model.Amount,
                        CollectedAmount = storyFromDb.CollectedAmount,
                        GoalAmount = storyFromDb.GoalAmount,
                        Donator = donator,
                        Title = storyFromDb.Title,
                        ErrorMessage = "Donation made!"
                    };
                }
                else
                {
                    resultModel = new DonateViewModel()
                    {
                        ImageUrl = storyFromDb.ImageUrl,
                        Amount = 0,
                        CollectedAmount = storyFromDb.CollectedAmount,
                        GoalAmount = storyFromDb.GoalAmount,
                        Donator = donator,
                        Title = storyFromDb.Title,
                        ErrorMessage = "Can not donate!"
                    };
                }
            }

            return this.View("Donate", resultModel);
        }
    }
}