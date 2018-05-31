namespace PayItForward.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;
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

        public void HardDelete(string id)
        {
            this.usersRepo.HardDeleteAsync(id);
            this.usersRepo.SaveAsync();
        }

        public async Task<Dbmodel.User> GetById(string id)
        {
            return await this.usersRepo.GetByIdAsync(id);
        }

        public Dbmodel.User GetByUserName(string userName)
        {
            return this.usersRepo.GetAll().FirstOrDefault(u => u.UserName == userName);
        }

        public Task SaveAsync()
        {
            return this.usersRepo.SaveAsync();
        }
    }
}
