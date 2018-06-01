namespace PayItForward.Services.Data.Abstraction
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dbmodel = PayItForward.Data.Models;

    public interface IUsersService
    {
        // to be changed based on Controller needs
        IQueryable<Dbmodel.User> GetAll();

        Task<Dbmodel.User> GetById(string id);

        Dbmodel.User GetByUserName(string userName);

        Task SaveAsync();

        Task HardDelete(string id);

        int Count();
    }
}
