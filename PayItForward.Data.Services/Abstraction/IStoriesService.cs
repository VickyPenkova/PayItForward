namespace PayItForward.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using PayItForward.Models;

    public interface IStoriesService
    {
        IEnumerable<StoryDTO> Stories(int take, int skip, string containsTitle = "", string isFromCategory = "");

        IEnumerable<StoryDTO> Stories();

        int CountStories(string containsTitle = "", string isFromCategory = "");

        StoryDTO GetStoryById(Guid id);

        Guid Add(StoryDTO story, string userId);
    }
}
