namespace PayItForward.Web.Models.StoryViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PayItForward.Common;
    using PayItForward.Models;

    public class AddStoryViewModel
    {
        [Required]
        [Display(Name = "Categories")]
        public List<SelectListItem> SelectCategory { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = GlobalConstants.CategoryAnimals, Text = "Animals" },
            new SelectListItem { Value = GlobalConstants.CategoryCharity, Text = "Charity" },
            new SelectListItem { Value = GlobalConstants.CategoryCreative, Text = "Creative" },
            new SelectListItem { Value = GlobalConstants.CategoryEducation, Text = "Education" },
            new SelectListItem { Value = GlobalConstants.CategoryEmergencies, Text = "Emergencies" },
            new SelectListItem { Value = GlobalConstants.CategoryFaith, Text = "Faith" },
            new SelectListItem { Value = GlobalConstants.CategoryHealth, Text = "Health" },
            new SelectListItem { Value = GlobalConstants.CategoryMemorials, Text = "Memorials" },
            new SelectListItem { Value = GlobalConstants.CategorySports, Text = "Sport" },
            new SelectListItem { Value = GlobalConstants.CategoryTravel, Text = "Travel" },
            new SelectListItem { Value = GlobalConstants.CategoryVolunteer, Text = "Volunteer" },
            new SelectListItem { Value = GlobalConstants.CategoryWishes, Text = "Wishes" }
        };

        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Story title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Goal Amount")]
        public decimal GoalAmount { get; set; }

        public string ErrorMessage { get; set; }

        [Display(Name = "Image url")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}
