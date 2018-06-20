namespace PayItForward.Services.Data.Abstraction
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Models;

    public interface IStoriesService
    {
        IEnumerable<StoryDTO> GetStories(int take, int skip, string containsTitle = "");

        IEnumerable<StoryDTO> GetStories();

        int CountStories(string containsTitle = "");
    }
}
