namespace PayItForward.ConsoleClient
{
    using System;

    public class ColorfulLoggerInfo : DetailedLoggerInfo
    {
        public ColorfulLoggerInfo(string name, ConsoleColor color, IConsoleWrapper consoleWrapper)
            : base(name, consoleWrapper)
        {
            this.ConsoleColor = color;
        }

        public override void PrintInfo(ILoggable loggable)
        {
            ConsoleColor previousColor = this.ConsoleWrapper.GetCurrentConsoleColor();
            this.ConsoleWrapper.ChangeColor(this.ConsoleColor);
            base.PrintInfo(loggable);
            this.ConsoleWrapper.ChangeColor(previousColor);
        }
    }
}
