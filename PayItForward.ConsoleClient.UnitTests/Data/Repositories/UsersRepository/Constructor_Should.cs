namespace PayItForward.UnitTests.Data.Repositories.UsersRepository
{
    using System;
    using PayItForward.Data;
    using Xunit;
    using Dbmodels = PayItForward.Data.Models;

    public class Constructor_Should
    {
        private readonly PayItForwardDbContext context;

        public Constructor_Should()
        {
            this.context = new PayItForwardDbContext();
        }

        [Fact]
        public void NotAcceptNullArgumentInConstructor()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentException>(() => new UsersRepository<Dbmodels.User, string>(null));
        }
    }
}
