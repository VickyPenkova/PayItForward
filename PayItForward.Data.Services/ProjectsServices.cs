namespace PayItForward.Services.Data
{
    using System.Linq;
    using PayItForward.Data;
    using PayItForward.Services.Data.Abstraction;
    using Dbmodel = PayItForward.Data.Models;

    public class ProjectsServices : IProjectService
    {
        private readonly IRepository<Dbmodel.User> users;

        public ProjectsServices()
        {
            this.users = new EfGenericRepository<Dbmodel.User>(new PayItForwardDbContext());
        }

        public IQueryable<Dbmodel.User> All()
        {
            throw new System.NotImplementedException();
        }
    }
}
