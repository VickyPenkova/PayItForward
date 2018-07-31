namespace PayItForward.Models
{
    using System;

    public class StoryDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string DocumentUrl { get; set; }

        public UserDTO User { get; set; }

        public DateTime ExpirationDate { get; set; }

        public CategoryDTO Category { get; set; }

        public decimal CollectedAmount { get; set; }

        public decimal GoalAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsRemoved { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsClosed { get; set; }
    }
}
