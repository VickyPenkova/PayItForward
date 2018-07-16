namespace PayItForward.Web.Models.HomeViewModels
{
    using System.Collections.Generic;
    using PayItForward.Web.Models.StoryViewModels;

    public class MyStoriesViewModel
    {
        public IEnumerable<BasicStoryViewModel> MyStories { get; set; }

        public string Message { get; set; }
    }
}
