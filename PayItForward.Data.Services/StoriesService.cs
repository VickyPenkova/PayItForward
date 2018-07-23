namespace PayItForward.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data;
    using PayItForward.Data.Models;
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
                storiesFromDb = this.storiesRepo.GetAllNotDeletedEntities()
                   .Include(user => user.User)
                   .Include(category => category.Category)
                   .Where(x => x.Title.Contains(subTitle))
                   .Where(st => st.Category.Name == categoryname)
                   .OrderBy(x => x.CreatedOn)
                   .Skip(skip)
                   .Take(take)
                   .ToList();
            }
            else
            {
                storiesFromDb = this.storiesRepo.GetAllNotDeletedEntities()
               .Include(user => user.User)
               .Include(category => category.Category)
               .Where(x => x.Title.Contains(subTitle))
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
                count = this.storiesRepo.GetAllNotDeletedEntities()
                .Where(x => x.Title.Contains(subTitle))
                .Where(st => st.Category.Name == categoryname)
                .Count();
            }
            else
            {
                count = this.storiesRepo.GetAllNotDeletedEntities()
                .Where(x => x.Title.Contains(subTitle))
                .Count();
            }

            return count;
        }

        public IEnumerable<StoryDTO> Stories()
        {
            var storiesFromDb = this.storiesRepo.GetAll()
                .Where(x => x.IsDeleted == false)
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
                .FirstOrDefault(s => s.Id == id);

            return this.mapper.Map<StoryDTO>(storiesFromDb);
        }

        public Guid Add(StoryDTO story, string userId)
        {
            var storyEntity = new Story
            {
                CategoryId = story.Category.Id,
                Title = story.Title,
                Description = story.Description,
                GoalAmount = story.GoalAmount,
                UserId = userId,
                ImageUrl = story.ImageUrl
            };

            this.storiesRepo.Add(storyEntity);
            this.storiesRepo.Save();

            return storyEntity.Id;
        }
    }
}
