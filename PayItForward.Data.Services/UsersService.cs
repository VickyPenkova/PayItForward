namespace PayItForward.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Data;
    using PayItForward.Services.Data.Abstraction;
    using Dbmodel = PayItForward.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly UsersRepository<Dbmodel.User, string> usersRepo;

        public UsersService()
        {
            this.usersRepo = new UsersRepository<Dbmodel.User, string>(new PayItForwardDbContext());
        }

        public IQueryable<Dbmodel.User> GetAll()
        {
            return this.usersRepo.GetAll().OrderBy(u => u.Id);
        }

        public int Count()
        {
            return this.usersRepo.GetAll().Count();
        }

        public async Task<Dbmodel.User> GetByIdAsync(string id)
        {
            return await this.usersRepo.GetByIdAsync(id);
        }

        public Dbmodel.User GetById(string id)
        {
            return this.usersRepo.GetById(id);
        }

        public Dbmodel.User GetByUserName(string userName)
        {
            return this.usersRepo.GetAll().FirstOrDefault(u => u.UserName == userName);
        }

        public Task SaveAsync()
        {
            return this.usersRepo.SaveAsync();
        }

        public Task<int> HardDeleteAsync(Dbmodel.User userTodelete)
        {
            throw new System.NotImplementedException();
        }

        public void SoftDelete(Dbmodel.User userTodelete)
        {
            this.usersRepo.SoftDelete(userTodelete);
        }
    }
}
