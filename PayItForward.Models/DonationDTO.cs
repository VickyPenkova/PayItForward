namespace PayItForward.Models
{
    using System;
    using PayItForward.Data.Models;

    public class DonationDTO : BaseModel<Guid>
    {
        public UserDTO Donator { get; set; }

        public StoryDTO Story { get; set; }

        public decimal Amount { get; set; }
    }
}