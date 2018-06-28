namespace PayItForward.Services.Abstraction
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PayItForward.Models;

    public interface ICategoriesService
    {
        IEnumerable<CategoryDTO> GetCategories();

        CategoryDTO GetCategoryById(Guid id);
    }
}
