namespace SlidoCodingAssessment.Helpers;

public sealed class LoggerUtils
{
    private static LoggerUtils _instance;
    private readonly List<string> _logs;

    private LoggerUtils()
    {
        _logs = new List<string>();
    }

    public static LoggerUtils Instance
    {
        get { return _instance ??= new LoggerUtils(); }
    }

    public void LogInfo(string message)
    {
        _logs.Add(@$"Info: {message}");
    }

    public void LogError(string message, string? exception = null)
    {
        _logs.Add(@$"Error: {message}");
        if (!string.IsNullOrWhiteSpace(exception)) _logs.Add(@$"Exception: {exception}");
    }

    public List<string> GetLogContent()
    {
        return _logs;
    }
}