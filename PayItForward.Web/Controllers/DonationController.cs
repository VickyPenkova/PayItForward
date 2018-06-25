namespace PayItForward.Web.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.DonationViewModels;
    using PayItForward.Web.Models.HomeViewModels;

    public class DonationController : Controller
    {
        private readonly IDonationsService donationsService;
        private readonly IStoriesService storiesService;
        private readonly IMapper mapper;

        public DonationController(IDonationsService donationsService, IStoriesService storiesService, IMapper mapper)
        {
            this.donationsService = donationsService;
            this.storiesService = storiesService;
            this.mapper = mapper;
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

                // TO DO: add donation here
                resultModel = this.mapper.Map<DonateViewModel>(storyFromDb);

                // resultModel.CollectedAmount += earnedMoney;
                // resultModel.Donations.Amount += earnedMoney;
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