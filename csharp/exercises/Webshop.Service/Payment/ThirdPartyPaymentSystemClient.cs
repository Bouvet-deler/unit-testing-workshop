namespace Webshop.Service.Payment;

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