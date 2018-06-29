namespace PayItForward.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PayItForward.Services.Abstraction;
    using PayItForward.Web.Models.CategoryViewModels;

    public class CategoryController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IMapper mapper;

        public CategoryController(ICategoriesService categoriesService, IMapper mapper)
        {
            this.categoriesService = categoriesService;
            this.mapper = mapper;
        }

        public IActionResult Index(Guid id)
        {
            var category = this.categoriesService.GetCategoryById(id);
            var model = this.mapper.Map<ListCategoriesViewModel>(category);
            return this.RedirectToAction("Index", "Home", model);
        }
    }
}