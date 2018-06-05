namespace PayItForward.ConsoleClient.UnitTests.DetailedLoggerInfo
{
    using Moq;
    using Xunit;

    public class PrintInfo_Should
    {
        private readonly Mock<IConsoleWrapper> consoleWrapper;
        private readonly ConsoleClient.DetailedLoggerInfo detailedLoggerInfo;

        public PrintInfo_Should()
        {
            this.consoleWrapper = new Mock<IConsoleWrapper>();
            this.detailedLoggerInfo = new ConsoleClient.DetailedLoggerInfo("DetailedLoggerInfo", this.consoleWrapper.Object);
        }

        [Fact]
        public void PrintBasicInfo()
        {
            // Arrange
            ILoggable user = new ConsoleClient.User("Viki", "Penkova", "123");

            // Act
            this.detailedLoggerInfo.PrintInfo(user);

            // Assert
            this.consoleWrapper.Verify(x => x.Print(user.LogBasicText), Times.Never);
            this.consoleWrapper.Verify(x => x.Print(user.LogDetailedText), Times.Once);
        }
    }
}
