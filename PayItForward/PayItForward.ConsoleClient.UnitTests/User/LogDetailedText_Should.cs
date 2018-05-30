namespace PayItForward.ConsoleClient
{
    using System;
    using Xunit;

    public class LogDetailedText_Should
    {
        private User user;

        public LogDetailedText_Should()
        {
            this.user = new User("Petia", "Asenova", "123");
        }

        [Fact]
        public void ReturnConcreteString()
        {
            string expected = $"First name:Petia\nLast name:Asenova\nId:123\nAmounts:0\n";
            Assert.Equal(expected, this.user.LogDetailedText);
        }
    }
}
