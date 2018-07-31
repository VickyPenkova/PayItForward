namespace PayItForward.Web.Models.StoryViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EditStoryViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Goal Amount")]
        public decimal GoalAmount { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        public DateTime ExpirationDate { get; set; }

        public Guid Id { get; set; }
    }
}
