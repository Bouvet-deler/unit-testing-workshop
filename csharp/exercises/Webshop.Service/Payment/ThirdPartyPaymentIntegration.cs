using Webshop.Service.Common;

namespace Webshop.Service.Payment;

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