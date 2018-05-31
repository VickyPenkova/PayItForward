namespace PayItForward.Services.Data
{
    using System.Linq;
    using PayItForward.Data;
    using PayItForward.Services.Data.Abstraction;
    using Dbmodel = PayItForward.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IRepository<Dbmodel.User> usersRepo;

        public UsersService()
        {
            this.usersRepo = new EfGenericRepository<Dbmodel.User>(new PayItForwardDbContext());
        }

        public IQueryable<Dbmodel.User> GetAll()
        {
            return this.usersRepo.GetAll().OrderBy(u => u.Id);
        }

        public int Count()
        {
            return this.usersRepo.GetAll().Count();
        }

        public void Delete(string id)
        {
            this.usersRepo.Delete(id);
            this.usersRepo.SaveChanges();
        }

        public Dbmodel.User GetById(string id)
        {
            return this.usersRepo.GetById(id);
        }

        public Dbmodel.User GetByUserName(string userName)
        {
            return this.usersRepo.GetAll().FirstOrDefault(u => u.UserName == userName);
        }

        public void Update()
        {
            this.usersRepo.SaveChanges();
        }
    }
}
