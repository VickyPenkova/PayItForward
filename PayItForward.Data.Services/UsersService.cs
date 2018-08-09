namespace PayItForward.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
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
            var usersFromDb = this.usersRepo
                .GetAll()
                .Take(count)
                .ToList();

            List<UserDTO> users = new List<UserDTO>();

            users = this.mapper.Map<List<UserDTO>>(usersFromDb);

            return users;
        }

        public int Count()
        {
            return this.usersRepo
                .GetAll()
                .Count();
        }

        public UserDTO GetUserById(string userId)
        {
            var userFromDb = this.usersRepo
                .GetById(userId);

            return this.mapper.Map<UserDTO>(userFromDb);
        }

        public IEnumerable<DonationDTO> GetDonations(string userId)
        {
            var user = this.usersRepo.GetAll()
                .Include(d => d.Donations)
                .Include(d => d.Stories)
                .FirstOrDefault(u => u.Id == userId);
            var donations = user.Donations;

            return this.mapper.Map<List<DonationDTO>>(donations);
        }

        public string Delete(string userId)
        {
            var userToDelete = this.usersRepo.GetAll()
                .Include(s => s.Stories)
                .FirstOrDefault(u => u.Id == userId);

             foreach (var story in userToDelete.Stories)
            {
                story.IsDeleted = true;
            }

            this.usersRepo.SoftDelete(userToDelete);

            this.usersRepo.Save();

            return string.Format("User with id {0} was deleted", userId);
        }
    }
}
