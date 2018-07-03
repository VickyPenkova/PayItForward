namespace PayItForward.Web.Models.HomeViewModels
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Web.Models.CategoryViewModels;
    using PayItForward.Web.Models.StoryViewModels;

    public class IndexViewModel
    {
        public IEnumerable<BasicStoryViewModel> Stories { get; set; }

        public IEnumerable<ListCategoriesViewModel> Categories { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string CurrentUrl { get; set; }

        public string SearchWord { get; set; }
    }
}
