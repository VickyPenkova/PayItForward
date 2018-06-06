namespace PayItForward.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using PayItForward.Data;
    using PayItForward.Models;
    using PayItForward.Services.Data.Abstraction;
    using PayItForwardDbmodels = PayItForward.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IRepository<PayItForwardDbmodels.User, string> usersRepo;

        // Constructor DI
        public UsersService(IRepository<PayItForwardDbmodels.User, string> usersRepo)
        {
            this.usersRepo = usersRepo;
        }

        public IEnumerable<UserDTO> GetUsers(int count)
        {
            var usersFromDb = this.usersRepo.GetAll().Take(count).ToList();

            List<UserDTO> users = new List<UserDTO>();

            foreach (var dbUser in usersFromDb)
            {
                users.Add(
                    new UserDTO()
                    {
                        FirstName = dbUser.FirstName,
                        LastName = dbUser.LastName,
                        AvilableMoneyAmount = dbUser.AvilableMoneyAmount,
                        AvatarUrl = dbUser.AvatarUrl
                    });
            }

            return users;
        }

        public int Count()
        {
            return this.usersRepo.GetAll().Count();
        }
    }
}
