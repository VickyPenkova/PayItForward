namespace PayItForward.Services.Data.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PayItForward.Models;

    public interface IStoriesService
    {
        IEnumerable<StoryDTO> GetStories(int take, int skip, string containsTitle = "");

        int CountStories(string containsTitle = "");
    }
}
