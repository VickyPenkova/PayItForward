namespace PayItForward.Web.Models.CategoryViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Models;

    public class ListCategoriesViewModel
    {
        public string Name { get; set; }

        public virtual Guid Id { get; set; }
    }
}
