namespace PayItForward.Models
{
    public class DonationDTO
    {
        public UserDTO User { get; set; }

        public StoryDTO Story { get; set; }

        public decimal Amount { get; set; }
    }
}