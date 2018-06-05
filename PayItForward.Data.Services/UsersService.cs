namespace PayItForward.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using PayItForward.Data;
    using PayItForward.Models;
    using PayItForward.Services.Data.Abstraction;
    using Dbmodel = PayItForward.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IRepository<Dbmodel.User, string> usersRepo;

        // Constructor DI
        public UsersService(IRepository<Dbmodel.User, string> usersRepo)
        {
            this.usersRepo = usersRepo;
        }

        public IEnumerable<UserDTO> GetUsers(int count)
        {
            var dbUsers = this.usersRepo.GetAll().Take(count).ToList();

            List<UserDTO> users = new List<UserDTO>();

            foreach (var dbUser in dbUsers)
            {
                users.Add(
                    new UserDTO()
                    {
                        FirstName = dbUser.FirstName,
                        LastName = dbUser.LastName,
                        AvilableMoneyAmount = dbUser.AvilableMoneyAmount,
                        AvatarUrl = dbUser.AvatarUrl
                    }
                  );
            }

            return users;
        }

        public int Count()
        {
            return this.usersRepo.GetAll().Count();
        }
    }
}
