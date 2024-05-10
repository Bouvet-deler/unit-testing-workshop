using Webshop.Service.Common;

namespace Webshop.Service.Payment;

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
