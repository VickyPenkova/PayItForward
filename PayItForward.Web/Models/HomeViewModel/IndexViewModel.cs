namespace PayItForward.Web.Models.HomeViewModel
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Web.Models.StoriesViewModel;

    public class IndexViewModel
    {
        public IEnumerable<BasicStoryViewModel> Stories { get; set; }

        public IEnumerable<BasicStoryViewModel> Users { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string CurrentUrl { get; set; }

        public string SearchWord { get; set; }

        public Guid Id { get; set; }
    }
}
