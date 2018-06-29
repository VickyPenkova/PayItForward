namespace PayItForward.Web.Models.StoryViewModels
{
    using System;
    using PayItForward.Models;

    public class DetailedStoryViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal GoalAmount { get; set; }

        public decimal CollectedAmount { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public UserDTO User { get; set; }

        public bool IsClosed { get; set; }

        public Guid Id { get; set; }
    }
}
