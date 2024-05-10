namespace Webshop.Service.Common;

public static class Logger
{
    public static void LogError(string message)
    {
        Log("Error", message);
    }

    public static void LogWarning(string message)
    {
        Log("Warning", message);
    }

    public static void LogInfo(string message)
    {
        Log("Info", message);
    }

    private static void Log(string level, string message)
    {
        using (var writer = File.AppendText("\\\\someserversomewhere\\log.txt"))
        {
            writer.WriteLine($"[{level}] [{DateTime.UtcNow.ToString("s")}] {message}");
        }
    }
}