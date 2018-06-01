namespace PayItForward.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Data;
    using PayItForward.Services.Data.Abstraction;
    using Dbmodel = PayItForward.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly UsersRepository<Dbmodel.User> usersRepo;

        public UsersService()
        {
            this.usersRepo = new UsersRepository<Dbmodel.User>(new PayItForwardDbContext());
        }

        public IQueryable<Dbmodel.User> GetAll()
        {
            return this.usersRepo.GetAll().OrderBy(u => u.Id);
        }

        public int Count()
        {
            return this.usersRepo.GetAll().Count();
        }

        public async Task HardDelete(string id)
        {
            await this.usersRepo.HardDeleteAsync(id);
            await this.usersRepo.SaveAsync();
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
