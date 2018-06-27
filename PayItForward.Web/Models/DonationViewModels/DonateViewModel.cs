namespace PayItForward.Web.Models.DonationViewModels
{
    using System.ComponentModel.DataAnnotations;
    using PayItForward.Models;

    public class DonateViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public UserDTO Donator { get; set; }

        public decimal GoalAmount { get; set; }

        public decimal CollectedAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Display(Name = "Donate")]
        public decimal Amount { get; set; }
    }
}
