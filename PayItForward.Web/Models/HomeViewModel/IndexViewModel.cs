namespace PayItForward.Web.Models.HomeViewModel
{
    using System.Collections.Generic;
    using PayItForward.Models;

    public class IndexViewModel
    {
        public IEnumerable<StoryDTO> Stories { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string CurrentUrl { get; set; }

        public string SearchWord { get; set; }
    }
}
