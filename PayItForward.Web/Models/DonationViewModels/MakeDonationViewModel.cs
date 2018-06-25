namespace PayItForward.Web.Models.DonationViewModels
{
    using PayItForward.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MakeDonationViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public decimal GoalAmount { get; set; }

        public decimal CollectedAmount { get; set; }

        public UserDTO User { get; set; }
    }
}
