namespace PayItForward.Models
{
    using System;

    public class DonationDTO
    {
        public UserDTO Donator { get; set; }

        public StoryDTO Story { get; set; }

        public decimal Amount { get; set; }

        public Guid Id { get; set; }
    }
}