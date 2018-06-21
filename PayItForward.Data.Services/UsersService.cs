namespace PayItForward.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using PayItForward.Data;
    using PayItForward.Models;
    using PayItForward.Services.Abstraction;
    using PayItForwardDbmodels = PayItForward.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IRepository<PayItForwardDbmodels.User, string> usersRepo;
        private readonly IMapper mapper;

        // Constructor DI
        public UsersService(IRepository<PayItForwardDbmodels.User, string> usersRepo, IMapper mapper)
        {
            this.usersRepo = usersRepo;
            this.mapper = mapper;
        }

        public IEnumerable<UserDTO> GetUsers(int count)
        {
            var usersFromDb = this.usersRepo.GetAll().Take(count).ToList();

            List<UserDTO> users = new List<UserDTO>();

            // var user = this.mapper.Map<UserDTO>(dbUser);
            users = this.mapper.Map<List<UserDTO>>(usersFromDb);
            return users;
        }

        public int Count()
        {
            return this.usersRepo.GetAll().Count();
        }

        public UserDTO GetUserById(string id)
        {
            var userFromDb = this.usersRepo.GetById(id);

            return this.mapper.Map<UserDTO>(userFromDb);
        }
    }
}
