namespace PayItForward.Services.Data.Abstraction
{
    using System.Collections.Generic;
    using PayItForward.Models;

    public interface IUsersService
    {
        IEnumerable<UserDTO> GetUsers(int count);

        UserDTO GetUserById(string id);

        int Count();
    }
}
