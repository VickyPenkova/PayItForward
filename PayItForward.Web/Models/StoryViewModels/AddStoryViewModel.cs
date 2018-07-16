namespace PayItForward.Web.Models.StoryViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PayItForward.Models;

    public class AddStoryViewModel
    {
        [Required]
        [Display(Name = "Categories")]
        public List<CategoryDTO> Categories { get; set; }

        [Required]
        [Display(Name = "Story title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Goal Amount")]
        public decimal GoalAmount { get; set; }

        public string ErrorMessage { get; set; }
    }
}
