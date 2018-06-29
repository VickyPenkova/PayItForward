namespace PayItForward.Web.Models.AdminViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Models;

    public class UserDetailsViewModel
    {
        public UserDTO User { get; set; }

        public IEnumerable<DonationDTO> Donations { get; set; }

        public IEnumerable<StoryDTO> Stories { get; set; }
    }
}
