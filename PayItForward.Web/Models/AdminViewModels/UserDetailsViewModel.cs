namespace PayItForward.Web.Models.AdminViewModels
{
    using System.Collections.Generic;
    using PayItForward.Models;
    using PayItForward.Web.Models.StoryViewModels;

    public class UserDetailsViewModel
    {
        public UserDTO User { get; set; }

        public IEnumerable<DonationDTO> Donations { get; set; }

        public IEnumerable<DetailedStoryViewModel> Stories { get; set; }
    }
}