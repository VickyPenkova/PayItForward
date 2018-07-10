namespace PayItForward.UnitTests.Data.Repositories.UsersRepository
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using PayItForward.Data;
    using PayItForward.Data.Abstraction;
    using Xunit;
    using Dbmodels = PayItForward.Data.Models;

    public class Add_Should
    {
        private readonly IRepository<Dbmodels.User, string> repo;
        private readonly Mock<IPayItForwardDbContext> context;

        public Add_Should()
        {
            this.context = new Mock<IPayItForwardDbContext>();
            this.repo = new UsersRepository<Dbmodels.User, string>(this.context.Object);
        }

        [Fact]
        public void ChangeEntityStateToStateAdded()
        {
            // Arrange
            var user = new Dbmodels.User()
            {
                UserName = "Helen",
                Email = "helen@gmail.com",
                FirstName = "Aleksandra",
                LastName = "Stoicheva",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Act
            this.repo.Add(user);

            // Assert
            this.context.Verify(c => c.ChangeEntityState(user, EntityState.Added), Times.Once);
        }
    }
}
