namespace PayItForward.ConsoleClient
{
    using System;

    public class ConsoleWrapper : IConsoleWrapper
    {
        public void Print(string text)
        {
            Console.Write(text);
        }

        public void ChangeColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public ConsoleColor GetCurrentConsoleColor()
        {
            return Console.ForegroundColor;
        }
    }
}
