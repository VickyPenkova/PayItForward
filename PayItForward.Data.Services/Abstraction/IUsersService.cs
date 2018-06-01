namespace PayItForward.Services.Data.Abstraction
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dbmodel = PayItForward.Data.Models;

    public interface IUsersService
    {
        // to be changed based on Controller needs
        IQueryable<Dbmodel.User> GetAll();

        // Task<Dbmodel.User> GetById(string id);
        Dbmodel.User GetById(string id);

        Task<Dbmodel.User> GetByIdAsync(string id);

        Dbmodel.User GetByUserName(string userName);

        Task SaveAsync();

        Task<int> HardDeleteAsync(Dbmodel.User userTodelete);

        void SoftDelete(Dbmodel.User userTodelete);

        int Count();
    }
}
