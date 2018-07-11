namespace PayItForward.Services.Abstraction
{
    using System.Collections.Generic;
    using PayItForward.Models;

    public interface IUsersService
    {
        IEnumerable<UserDTO> GetUsers(int count);

        UserDTO GetUserById(string userId);

        int Count();

        IEnumerable<DonationDTO> GetDonations(string userId);

        string Delete(string userId);
    }
}
