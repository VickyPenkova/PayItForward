namespace PayItForward.ConsoleClient
{
    using System;
    using Moq;
    using Xunit;

    public class PrintInfo_Should
    {
        private readonly Mock<IConsoleWrapper> consoleWrapper;
        private readonly BasicLoggerInfo basicLoggerInfo;

        public PrintInfo_Should()
        {
            this.consoleWrapper = new Mock<IConsoleWrapper>();
            this.basicLoggerInfo = new BasicLoggerInfo("BasicLoggerInfo", this.consoleWrapper.Object);
        }

        [Fact]
        public void PrintBasicInfo()
        {
            // Arrange
            ILoggable user = new User("Viki", "Penkova", "123");

            // Act
            this.basicLoggerInfo.PrintInfo(user);

            // Assert
            this.consoleWrapper.Verify(x => x.Print(user.LogBasicText), Times.Once);
        }
    }
}
