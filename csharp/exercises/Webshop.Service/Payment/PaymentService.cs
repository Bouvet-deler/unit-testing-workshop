using Webshop.Service.Common;
using Webshop.Service.Users;

namespace Webshop.Service.Payment;

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