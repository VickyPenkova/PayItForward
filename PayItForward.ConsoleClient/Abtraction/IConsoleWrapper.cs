namespace PayItForward.ConsoleClient
{
    using System;

    public interface IConsoleWrapper
    {
        void Print(string text);

        void ChangeColor(ConsoleColor color);

        ConsoleColor GetCurrentConsoleColor();
    }
}
