using System;

namespace PayItForward.Models
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public double AvilableMoneyAmount { get; set; }

        public string AvatarUrl { get; set; }
    }
}
