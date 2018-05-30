namespace PayItForward.ConsoleClient
{
    using System;
    using EnsureThat;

    public class User : ILoggable
    {
        public User(string firstName, string lastName, string id)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Id = id;

            Ensure.That(firstName).IsNotNullOrEmpty();
            Ensure.That(lastName).IsNotNullOrEmpty();
        }

        public string LogBasicText
        {
            get
            {
                return $"First name:{this.FirstName}\nLast name:{this.LastName}\n";
            }
        }

        public string LogDetailedText
        {
            get
            {
                return this.LogBasicText + $"Id:{this.Id}\nAmounts:{this.AvilableMoneyAmount}\n";
            }
        }

        protected string Id { get; }

        protected double AvilableMoneyAmount { get; private set; }

        protected string FirstName { get; private set; }

        protected string LastName { get; private set; }
    }
}
