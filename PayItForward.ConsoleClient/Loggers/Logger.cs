namespace PayItForward.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using EnsureThat;

    public abstract class Logger
    {
        private readonly IConsoleWrapper consoleWrapper;

        public Logger(string name, IConsoleWrapper consoleWrapper)
        {
            this.consoleWrapper = consoleWrapper;
            this.Name = name;

            Ensure.That(name).IsNotNullOrEmpty();
        }

        public ConsoleColor ConsoleColor { get; set; }

        protected IConsoleWrapper ConsoleWrapper
        {
            get
            {
                return this.consoleWrapper;
            }
        }

        protected List<User> Users { get; }

        protected string Name { get; private set; }

        public string LoggerName()
        {
            return $"Logger Name: {this.Name}\n";
        }

        public abstract void PrintInfo(ILoggable loggable);

        public void PrintInfoList(List<ILoggable> loggables)
        {
            if (loggables is null || loggables.Count == 0)
            {
                throw new ArgumentNullException("No loggers");
            }

            foreach (var loggable in loggables)
            {
                this.PrintInfo(loggable);
            }
        }
    }
}
