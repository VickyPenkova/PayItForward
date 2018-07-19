namespace PayItForward.Web.Models.StoryViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PayItForward.Models;

    public class AddStoryViewModel
    {
        [Display(Name = "Categories")]
        public IEnumerable<CategoryDTO> Categories { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Story title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Goal Amount")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal GoalAmount { get; set; }

        [Display(Name = "Image url")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}
