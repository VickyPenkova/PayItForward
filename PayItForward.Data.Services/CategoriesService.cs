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

    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<PayItForward.Data.Models.Category, Guid> categoriesRepo;
        private readonly IMapper mapper;

        public CategoriesService(IRepository<PayItForward.Data.Models.Category, Guid> categoriesRepo, IMapper mapper)
        {
            this.categoriesRepo = categoriesRepo;
            this.mapper = mapper;
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var categoriesFromDb = this.categoriesRepo.GetAll().ToList();

            List<CategoryDTO> categories = new List<CategoryDTO>();

            categories = this.mapper.Map<List<CategoryDTO>>(categoriesFromDb);

            return categories;
        }

        public CategoryDTO GetCategoryById(Guid id)
        {
            var categoriesFromDb = this.categoriesRepo.GetAll()
                .Include(c => c.Stories)
                .Where(c => c.Id == id).FirstOrDefault();

            return this.mapper.Map<CategoryDTO>(categoriesFromDb);
        }
    }
}
