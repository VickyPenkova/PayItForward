﻿namespace PayItForward.Web.Models.HomeViewModels
{
    using System;
    using PayItForward.Models;

    public class DetailsViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public decimal GoalAmount { get; set; }

        public decimal CollectedAmount { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public UserDTO User { get; set; }

        // Not used in details
        public DonationDTO Donations { get; set; }

        public bool IsClosed { get; set; }

        public Guid Id { get; set; }
    }
}
