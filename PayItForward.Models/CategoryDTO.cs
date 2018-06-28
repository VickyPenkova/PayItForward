namespace PayItForward.Models
{
    using System;

    public class CategoryDTO
    {
        public string Name { get; set; }

        public virtual Guid Id { get; set; }
    }
}
