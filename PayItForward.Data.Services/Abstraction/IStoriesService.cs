namespace PayItForward.Services.Data.Abstraction
{
    using System.Collections.Generic;
    using System.Linq;
    using PayItForward.Models;

    public interface IStoriesService
    {
        IEnumerable<StoryDTO> GetStories(int take, int skip, string containsTitle = "");

        int CountStories(string containsTitle = "");
    }
}
