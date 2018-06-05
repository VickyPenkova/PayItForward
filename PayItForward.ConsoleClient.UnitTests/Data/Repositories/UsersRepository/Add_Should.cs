namespace PayItForward.UnitTests.Data.Repositories.UsersRepository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using PayItForward.Data;
    using Xunit;
    using Dbmodels = PayItForward.Data.Models;

    public class Add_Should
    {
        private readonly UsersRepository<Dbmodels.User, string> usersRepository;
        private readonly Mock<IRepository<Dbmodels.User, string>> repo;

        public Add_Should()
        {
            this.repo = new Mock<IRepository<Dbmodels.User, string>>();
        }

        private DbSet<Dbmodels.User> DbSet { get; set; }

        [Fact]
        public void CallChangeEntityStateOnce()
        {
            // Arrange


            // Act

            // Assert
            // this.repo.Verify(x => x.Add(), Times.Once);
        }
    }
}
