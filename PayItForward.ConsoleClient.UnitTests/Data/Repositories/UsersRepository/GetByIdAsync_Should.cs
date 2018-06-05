namespace PayItForward.UnitTests.Data.Repositories.UsersRepository
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data;
    using Xunit;
    using Dbmodels = PayItForward.Data.Models;

    public class GetByIdAsync_Should
    {
        private readonly UsersRepository<Dbmodels.User, string> usersRepository;

        public GetByIdAsync_Should()
        {
            this.usersRepository = new UsersRepository<Dbmodels.User, string>(new PayItForwardDbContext());
        }

        private DbSet<Dbmodels.User> DbSet { get; set; }
    }
}
