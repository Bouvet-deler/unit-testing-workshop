using Webshop.Service.Common;
using Webshop.Service.Users;

namespace Webshop.Service.Payment;

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