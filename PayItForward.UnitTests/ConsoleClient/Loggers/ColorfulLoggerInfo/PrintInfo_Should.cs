namespace PayItForward.ConsoleClient.UnitTests.ColorfulLoggerInfo
{
    using System;
    using Moq;
    using Xunit;

    public class PrintInfo_Should
    {
        private readonly ConsoleClient.ColorfulLoggerInfo colorfulLoggerInfo;
        private readonly Mock<IConsoleWrapper> consoleWrapperMock;
        private readonly ILoggable user;

        public PrintInfo_Should()
        {
            this.consoleWrapperMock = new Mock<IConsoleWrapper>();
            this.user = new ConsoleClient.User("Deni", "Doncheva", "123");
            this.colorfulLoggerInfo = new ConsoleClient.ColorfulLoggerInfo("ColorfulLoggerInfo", ConsoleColor.Cyan, this.consoleWrapperMock.Object);
        }

        [Fact]
        public void RestorePreviousColorCallingChangeColor()
        {
            // Arrange
            ConsoleColor expectedColor = ConsoleColor.Green;
            this.consoleWrapperMock.Setup(x => x.GetCurrentConsoleColor()).Returns(expectedColor);

            // Act
            this.colorfulLoggerInfo.PrintInfo(this.user);

            // Assert
            this.consoleWrapperMock.Verify(x => x.ChangeColor(expectedColor), Times.Once);
        }

        [Fact]
        public void CallPrintMethod_With_LogDetailedText()
        {
            // Act
            this.colorfulLoggerInfo.PrintInfo(this.user);

            // Assert
            this.consoleWrapperMock.Verify(x => x.Print(this.user.LogDetailedText), Times.Once);
        }
    }
}
