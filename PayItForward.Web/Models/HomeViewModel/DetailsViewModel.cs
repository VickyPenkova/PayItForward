namespace PayItForward.Web.Models.HomeViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Web.Models.StoriesViewModel;

    public class DetailsViewModel
    {
        public DetailedStoryViewModel Story { get; set; }

        public string CurrentUrl { get; set; }
    }
}
