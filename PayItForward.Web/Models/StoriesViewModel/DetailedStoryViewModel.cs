namespace PayItForward.Web.Models.StoriesViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Models;
    using PayItForward.Web.Infrastructure.AutoMapper;

    public class DetailedStoryViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public decimal GoalAmount { get; set; }

        public decimal CollectedAmount { get; set; }

        public bool IsClosed { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public CategoryDTO Category { get; set; }

        public UserDTO User { get; set; }

        public string DocumentUrl { get; set; }

        public DonationDTO Donations { get; set; }

        public bool IsRemoved { get; set; }
    }
}
