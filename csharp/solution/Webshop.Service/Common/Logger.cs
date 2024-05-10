namespace Webshop.Service.Common;

public interface ILogger
{
    void LogError(string message);
    void LogWarning(string message);
    void LogInfo(string message);
}

public class Logger : ILogger
{
    public void LogError(string message)
    {
        Log("Error", message);
    }

    public void LogWarning(string message)
    {
        Log("Warning", message);
    }

    public void LogInfo(string message)
    {
        Log("Info", message);
    }

    private void Log(string level, string message)
    {
        using (var writer = File.AppendText("\\someserversomewhere\\log.txt"))
        {
            writer.WriteLine($"[{level}] [{DateTime.UtcNow.ToString("s")}] {message}");
        }
    }
}