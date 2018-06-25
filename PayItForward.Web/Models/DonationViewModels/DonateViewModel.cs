namespace PayItForward.Web.Models.DonationViewModels
{
    using System;
    using PayItForward.Models;

    public class DonateViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public decimal GoalAmount { get; set; }

        public decimal CollectedAmount { get; set; }

        public UserDTO User { get; set; }

        public Guid Id { get; set; }
    }
}
