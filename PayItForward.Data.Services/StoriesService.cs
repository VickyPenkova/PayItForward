﻿namespace PayItForward.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
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
            var storiesFromDb = this.storiesRepo.GetAll()
               .Include(user => user.User)
               .Include(category => category.Category)
               .Where(x => x.Title.Contains(containsTitle))
               .OrderBy(x => x.CreatedOn)
               .Skip(skip)
               .Take(take)
               .ToList();

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