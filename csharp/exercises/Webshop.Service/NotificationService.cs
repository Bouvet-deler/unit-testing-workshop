namespace Webshop.Service;

public interface INotificationService
{
    void SendMail(string recipientEmail, string message);
}

public class NotificationService : INotificationService
{
    public void SendMail(string recipientEmail, string message)
    {
        // Send mail to the specified email-address
        // Uses an email-server which is not available from our developer environment
        Thread.Sleep(int.MaxValue);
    }
}