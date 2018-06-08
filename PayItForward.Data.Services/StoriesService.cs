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

        public List<StoryDTO> GetStories(int count)
        {
            var storiesFromDb = this.storiesRepo.GetAll().Take(count);

            List<StoryDTO> stories = new List<StoryDTO>();

            stories = this.mapper.Map<List<StoryDTO>>(storiesFromDb);

            return stories;
        }
    }
}
