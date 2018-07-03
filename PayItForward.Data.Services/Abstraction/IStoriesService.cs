namespace PayItForward.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Models;

    public interface IStoriesService
    {
        IEnumerable<StoryDTO> GetStories(int take, int skip, string containsTitle = "", string isFromCategory = "");

        IEnumerable<StoryDTO> GetStories();

        int CountStories(string containsTitle = "", string isFromCategory = "");

        StoryDTO GetStoryById(Guid id);

        IEnumerable<StoryDTO> GetStories(int take, int skip, string containsTitle = "");

        int CountStories(string containsTitle = "");

    }
}
