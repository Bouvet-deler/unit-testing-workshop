package java.Webshop.Service.Common;

interface INotificationService {
    void sendMail(String recipientEmail, String message);
}

public class NotificationService implements INotificationService {
    public void sendMail(String recipientEmail, String message) {
        // Simulate sending mail (since the email server is not available)
        try {
            Thread.sleep(Integer.MAX_VALUE);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }
}