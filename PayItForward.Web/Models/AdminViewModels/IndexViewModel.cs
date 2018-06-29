namespace PayItForward.Web.Models.AdminViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Web.Models.StoryViewModels;

    public class IndexViewModel
    {
        public IEnumerable<BasicStoryViewModel> Stories { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string CurrentUrl { get; set; }

        public string SearchWord { get; set; }

        public Guid Id { get; set; }
    }
}
