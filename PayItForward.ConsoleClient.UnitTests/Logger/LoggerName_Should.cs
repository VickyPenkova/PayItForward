namespace PayItForward.ConsoleClient
{
    using Xunit;

    public class LoggerName_Should
    {
        private IConsoleWrapper consoleWrapper;

        public LoggerName_Should()
        {
            this.consoleWrapper = new ConsoleWrapper();
        }

        [Fact]
        public void ReturnConcreteString()
        {
            Logger basicLoggerinfo = new BasicLoggerInfo("BasicLoggerInfo", this.consoleWrapper);
            string expected = $"Logger Name: BasicLoggerInfo\n";

            Assert.Equal(expected, basicLoggerinfo.LoggerName());
        }
    }
}
