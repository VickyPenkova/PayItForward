namespace PayItForward.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category : BaseModel
    {
        private readonly ICollection<Story> stories;

        public Category()
        {
            this.stories = new HashSet<Story>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<Story> Stories => this.stories;

        public bool IsRemoved { get; set; }
    }
}
