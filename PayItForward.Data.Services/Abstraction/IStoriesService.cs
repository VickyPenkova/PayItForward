namespace PayItForward.Services.Data.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PayItForward.Models;

    public interface IStoriesService
    {
        List<StoryDTO> GetStories(int count);
    }
}
