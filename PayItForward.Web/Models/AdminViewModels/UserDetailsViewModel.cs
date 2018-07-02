namespace PayItForward.Web.Models.AdminViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Models;
    using PayItForward.Web.Models.StoryViewModels;

    public class UserDetailsViewModel
    {
        public UserDTO User { get; set; }

        public IEnumerable<DonationDTO> Donations { get; set; }

        public List<DetailedStoryViewModel> Stories { get; set; }
    }
}
