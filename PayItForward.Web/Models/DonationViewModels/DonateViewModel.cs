namespace PayItForward.Web.Models.DonationViewModels
{
    using System;
    using PayItForward.Models;

    public class DonateViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DonationDTO Donation { get; set; }

        public Guid StoryId { get; set; }
    }
}
