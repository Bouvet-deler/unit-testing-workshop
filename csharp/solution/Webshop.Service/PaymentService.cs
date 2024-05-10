namespace Webshop.Service;

public interface IPaymentService
{
    PaymentResult HandlePayment(string userId, decimal amountInNok);
}

public class PaymentService : IPaymentService
{
    private readonly IThirdPartyPaymentIntegration _thirdPartyPaymentIntegration;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly ILogger? _logger;

    public PaymentService(IThirdPartyPaymentIntegration thirdPartyPaymentIntegration, IUserService userService, INotificationService notificationService, ILogger? logger)
    {
        _thirdPartyPaymentIntegration = thirdPartyPaymentIntegration;
        _userService = userService;
        _notificationService = notificationService;
        _logger = logger;
    }

    public PaymentResult HandlePayment(string userId, decimal amountInNok)
    {
        var user = _userService.GetUser(userId);
        if (user == null)
        {
            var msg = $"Attempted to handle payment for non-existing user {userId}";
            _logger?.LogError(msg);
            return new PaymentResult { Code = PaymentResultCode.UserNotFound, ErrorMessage = msg };
        }

        if (amountInNok < 0)
        {
            return new PaymentResult { Code = PaymentResultCode.InvalidAmount, ErrorMessage = "Amount cannot be negative" };
        }

        var paymentOk = _thirdPartyPaymentIntegration.HandlePaymentForUser(userId, amountInNok, "WebShop payment");
        if (!paymentOk)
        {
            var msg = $"Payment failed for user {userId} with amount {amountInNok}";
            _logger?.LogError(msg);
            return new PaymentResult { Code = PaymentResultCode.PaymentIntegrationFailed, ErrorMessage = msg };
        }

        _notificationService.SendMail(user.Email, "Payment succeeded");

        return new PaymentResult { Code = PaymentResultCode.Success };
    }
}

public class PaymentResult
{
    public bool Success => Code == PaymentResultCode.Success;
    public PaymentResultCode Code { get; set; }
    public string? ErrorMessage { get; set; }
}

public enum PaymentResultCode
{
    Success,
    UserNotFound,
    InvalidAmount,
    PaymentIntegrationFailed
}

public interface IThirdPartyPaymentIntegration
{
    bool HandlePaymentForUser(string userId, decimal amountInNok, string message);
}

public class ThirdPartyPaymentIntegration : IThirdPartyPaymentIntegration
{
    private readonly IThirdPartyPaymentSystemClient _client;
    private readonly ILogger _logger;

    public ThirdPartyPaymentIntegration(IThirdPartyPaymentSystemClient client, ILogger logger)
    {
        _client = client;
        _logger = logger;
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
            _logger.LogError($"Payment failed: {ex.Message}");
            return false;
        }
    }
}

public interface IThirdPartyPaymentSystemClient
{
    bool HandlePayment(string userId, decimal amountInNok, string message);
    void WaitForConnection();
}

public class ThirdPartyPaymentSystemClient : IThirdPartyPaymentSystemClient
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