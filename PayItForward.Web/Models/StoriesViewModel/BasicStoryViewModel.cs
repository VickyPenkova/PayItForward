namespace PayItForward.Web.Models.StoriesViewModel
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using PayItForward.Models;

    public class BasicStoryViewModel
    {
        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public UserDTO User { get; set; }

        public CategoryDTO Category { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal CollectedAmount { get; set; }
    }
}
