namespace PayItForward.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using PayItForward.Data;
    using PayItForward.Models;
    using PayItForward.Services.Data.Abstraction;
    using PayItForwardDbmodels = PayItForward.Data.Models;

    public class StoriesService : IStoriesService
    {
        private readonly IRepository<PayItForwardDbmodels.Story, Guid> storiesRepo;
        private readonly IMapper mapper;

        // Constructor DI
        public StoriesService(IRepository<PayItForwardDbmodels.Story, Guid> storiesRepo, IMapper mapper)
        {
            this.storiesRepo = storiesRepo;
            this.mapper = mapper;
        }

        public IEnumerable<StoryDTO> GetStories(int take, int skip, string containsTitle = "")
        {
            var storiesFromDb = this.storiesRepo.GetAll().Where(x => x.Title.Contains(containsTitle)).Skip(skip).Take(take);

            List<StoryDTO> stories = new List<StoryDTO>();

            stories = this.mapper.Map<List<StoryDTO>>(storiesFromDb);

            return stories;
        }

        public int CountStories(string containsTitle = "")
        {
            return this.storiesRepo.GetAll().Where(x => x.Title.Contains(containsTitle)).Count();
        }
    }
}
