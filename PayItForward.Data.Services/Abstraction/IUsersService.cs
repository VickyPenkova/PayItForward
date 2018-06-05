namespace PayItForward.Services.Data.Abstraction
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Data;
    using PayItForward.Models;
    using Dbmodels = PayItForward.Data.Models;

    public interface IUsersService
    {
        IEnumerable<UserDTO> GetUsers(int count);

        int Count();
    }
}
