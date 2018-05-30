namespace PayItForward.Services.Data.Abstraction
{
    using System.Linq;
    using Dbmodel = PayItForward.Data.Models;

    public interface IProjectService
    {
        // to be chnaged based on Controller needs
        IQueryable<Dbmodel.User> All();
    }
}
