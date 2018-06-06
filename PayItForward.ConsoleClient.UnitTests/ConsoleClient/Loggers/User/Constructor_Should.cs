namespace PayItForward.ConsoleClient.UnitTests.User
{
    using System;
    using Xunit;

    public class Constructor_Should
    {
        private IConsoleWrapper consoleWrapper;

        public Constructor_Should()
        {
            this.consoleWrapper = new ConsoleWrapper();
        }

        [Fact]
        public void NotAcceptEmptyNameInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new PayItForward.ConsoleClient.User(null, null, null));
        }
    }
}
