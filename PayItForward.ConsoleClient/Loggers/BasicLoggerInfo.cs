namespace PayItForward.ConsoleClient
{
    using System;

    public class BasicLoggerInfo : Logger
    {
        public BasicLoggerInfo(string name, IConsoleWrapper consoleWrapper)
            : base(name, consoleWrapper)
        {
        }

        public override void PrintInfo(ILoggable loggable)
        {
            try
            {
                this.ConsoleWrapper.Print(loggable.LogBasicText);
            }
            catch (Exception ex)
            {
                this.ConsoleWrapper.Print(ex.Message);
            }
        }
    }
}
