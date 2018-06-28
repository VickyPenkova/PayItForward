namespace PayItForward.Web.Models.DonationViewModels
{
    using System.ComponentModel.DataAnnotations;
    using PayItForward.Models;

    public class DonateViewModel
    {
        public virtual string Title { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual UserDTO Donator { get; set; }

        public virtual decimal GoalAmount { get; set; }

        public virtual decimal CollectedAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Display(Name = "Donate")]
        public virtual decimal Amount { get; set; }

        public virtual string ErrorMessage { get; set; }
}
}
