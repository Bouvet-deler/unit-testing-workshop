namespace Webshop.Service;

public interface IPaymentService
{
    void HandlePayment(string userId, decimal amountInNok);
}

public class PaymentService : IPaymentService
{
    private readonly ThirdPartyPaymentIntegration _thirdPartyPaymentIntegration;

    public PaymentService()
    {
        _thirdPartyPaymentIntegration = new ThirdPartyPaymentIntegration();
    }

    public void HandlePayment(string userId, decimal amountInNok)
    {
        var userService = new UserService(new UserRepository(), new PasswordValidator());
        var user = userService.GetUser(userId);
        if (user == null)
        {
            Logger.LogError($"Attempted to handle payment for non-existing user {userId}");
            throw new Exception($"Could not find user with id {userId}");
        }

        if (amountInNok < 0)
            throw new Exception("Amount cannot be negative");

        var paymentOk = _thirdPartyPaymentIntegration.HandlePaymentForUser(userId, amountInNok, "WebShop payment");
        if (!paymentOk)
        {
            Logger.LogError($"Payment failed for user {userId} with amount {amountInNok}");
            throw new Exception("Payment failed");
        }

        var notificationService = new NotificationService();
        notificationService.SendMail(user.Email, "Payment succeeded");
    }
}

public class ThirdPartyPaymentIntegration
{
    private readonly ThirdPartyPaymentSystemClient _client;

    public ThirdPartyPaymentIntegration()
    {
        _client = new ThirdPartyPaymentSystemClient();
    }

    public bool HandlePaymentForUser(string userId, decimal amountInNok, string message)
    {
        try
        {
            _client.WaitForConnection();
            return _client.HandlePayment(userId, amountInNok, message);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Payment failed: {ex.Message}");
            return false;
        }
    }
}

public class ThirdPartyPaymentSystemClient
{
    public void WaitForConnection()
    {
        // Simulate that we need to connect to a third party system which is not reachable from non-production environment
        Thread.Sleep(TimeSpan.FromDays(365));
    }

    public bool HandlePayment(string userId, decimal amountInNok, string message)
    {
        // Simulate some work that may take some time
        Thread.Sleep(TimeSpan.FromSeconds(5));
        return true;
    }
}

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