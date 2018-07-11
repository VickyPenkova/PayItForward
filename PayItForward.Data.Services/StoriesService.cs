namespace PayItForward.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
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

        public IEnumerable<StoryDTO> Stories(int take, int skip, string subTitle = "", string categoryname = "")
        {
            var isCategoryNull = string.IsNullOrEmpty(categoryname);
            List<PayItForwardDbmodels.Story> storiesFromDb;

            if (!isCategoryNull)
            {
                storiesFromDb = this.storiesRepo.GetAll()
                   .Include(user => user.User)
                   .Include(category => category.Category)
                   .Where(x => x.Title.Contains(subTitle))
                   .Where(st => st.Category.Name == categoryname && st.IsDeleted == false)
                   .OrderBy(x => x.CreatedOn)
                   .Skip(skip)
                   .Take(take)
                   .ToList();
            }
            else
            {
                storiesFromDb = this.storiesRepo.GetAll()
               .Include(user => user.User)
               .Include(category => category.Category)
               .Where(x => x.Title.Contains(subTitle) && x.IsDeleted == false)
               .OrderBy(x => x.CreatedOn)
               .Skip(skip)
               .Take(take)
               .ToList();
            }

            List<StoryDTO> stories = new List<StoryDTO>();
            stories = this.mapper.Map<List<StoryDTO>>(storiesFromDb);
            return stories;
        }

        public int CountStories(string subTitle = "", string categoryname = "")
        {
            int count;
            if (!string.IsNullOrEmpty(categoryname))
            {
                count = this.storiesRepo.GetAll()
                .Where(x => x.Title.Contains(subTitle) && x.IsDeleted == false)
                .Where(st => st.Category.Name == categoryname)
                .Count();
            }
            else
            {
                count = this.storiesRepo.GetAll()
                .Where(x => x.Title.Contains(subTitle) && x.IsDeleted == false)
                .Count();
            }

            return count;
        }

        public IEnumerable<StoryDTO> Stories()
        {
            var storiesFromDb = this.storiesRepo.GetAll()
                .Include(user => user.User)
                .Include(category => category.Category);

            List<StoryDTO> stories = new List<StoryDTO>();
            stories = this.mapper.Map<List<StoryDTO>>(storiesFromDb);
            return stories;
        }

        public StoryDTO GetStoryById(Guid id)
        {
            var storiesFromDb = this.storiesRepo.GetAll()
                .Include(s => s.User)
                .Where(s => s.Id == id)
                .FirstOrDefault();

            return this.mapper.Map<StoryDTO>(storiesFromDb);
        }
    }
}
