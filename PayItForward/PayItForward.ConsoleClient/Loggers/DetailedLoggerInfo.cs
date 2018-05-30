namespace PayItForward.ConsoleClient
{
    using System;

    public class DetailedLoggerInfo : BasicLoggerInfo
    {
        public DetailedLoggerInfo(string name, IConsoleWrapper consoleWrapper)
            : base(name, consoleWrapper)
        {
        }

        public override void PrintInfo(ILoggable loggable)
        {
            try
            {
                this.ConsoleWrapper.Print(loggable.LogDetailedText);
            }
            catch (Exception ex)
            {
                this.ConsoleWrapper.Print(ex.Message);
            }
        }
    }
}
