namespace PayItForward.Services.Data.Abstraction
{
    using System.Linq;
    using Dbmodel = PayItForward.Data.Models;

    public interface IUsersService
    {
        // to be changed based on Controller needs
        IQueryable<Dbmodel.User> GetAll();

        Dbmodel.User GetById(string id);

        Dbmodel.User GetByUserName(string userName);

        void Update();

        void Delete(string id);

        int Count();
    }
}
