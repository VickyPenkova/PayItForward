namespace PayItForward.Models
{
    public class UserDTO
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

        public virtual decimal AvilableMoneyAmount { get; set; }

        public virtual string AvatarUrl { get; set; }

        public virtual string Id { get; set; }
    }
}
