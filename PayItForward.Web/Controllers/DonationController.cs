﻿namespace PayItForward.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.DonationViewModels;

    public class DonationController : Controller
    {
        private readonly IDonationsService donationsService;
        private readonly IStoriesService storiesService;
        private readonly IUsersService usersService;
        private readonly IMapper mapper;

        public DonationController(
            IDonationsService donationsService,
            IStoriesService storiesService,
            IUsersService usersService,
            IMapper mapper)
        {
            this.donationsService = donationsService;
            this.storiesService = storiesService;
            this.usersService = usersService;
            this.mapper = mapper;
        }

        public IActionResult Donate(Guid id)
        {
            var storyFromDb = this.storiesService.GetStoryById(id);

            if (storyFromDb == null)
            {
                return this.Content("Story not found.");
            }

            // TODO: Can not be xUnit tested
            UserDTO test = this.usersService.GetUserById(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

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

            var donatorId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var donator = this.usersService.GetUserById(donatorId);
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
                        ErrorMessage = "You've just made a donation!"
                    };
                }
                else
                {
                    this.TempData["message"] = "Can not donate!";
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