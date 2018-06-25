namespace PayItForward.Web.Controllers
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
    using PayItForwardDbmodels = PayItForward.Data.Models;

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
            var storyFromDb = this.storiesService.GetStories().Where(s => s.Id == id).FirstOrDefault();
            var donation = new DonationDTO()
            {
                Story = storyFromDb,
                Donator = this.usersService.GetUserById(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            var d = new DonateViewModel()
            {
                Donation = donation,
                StoryId = id,
                Title = storyFromDb.Title,
                ImageUrl = storyFromDb.ImageUrl
            };

            var storyToDonate = this.mapper.Map<DonateViewModel>(d);

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

                var donatorId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var donator = this.usersService.GetUserById(donatorId);

                var donation = new DonationDTO()
                {
                    Amount = moneyToDonate,
                    Story = storyFromDb,
                    Donator = donator
                };

                var makeDonation = this.donationsService.Add(donation, id);

                // TO DO: add donation here
                resultModel = new DonateViewModel()
                {
                    ImageUrl = storyFromDb.ImageUrl,
                    Donation = donation,
                    Title = storyFromDb.Title
                };

                this.TempData["message"] = "You have just made a donation!";
            }
            else
            {
                this.TempData["message"] = "Not enough availability!";
            }

            // return this.Redirect(this.ControllerContext.HttpContext.Request.Headers["Referer"].ToString());
            return this.View("Donate", resultModel);
        }
    }
}