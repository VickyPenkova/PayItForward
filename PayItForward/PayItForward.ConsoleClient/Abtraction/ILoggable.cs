namespace PayItForward.ConsoleClient
{
    public interface ILoggable
    {
        string LogBasicText { get; }

        string LogDetailedText { get; }
    }
}
