using System;
using System.Collections.Generic;
using System.Text;

namespace PayItForward.Models
{
    public class StoryDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public UserDTO User { get; set; }

        public DateTime ExpirationDate { get; set; }

        public CategoryDTO Category { get; set; }

        public decimal CollectedAmount { get; set; }

        public bool IsClosed { get; set; }
    }
}
