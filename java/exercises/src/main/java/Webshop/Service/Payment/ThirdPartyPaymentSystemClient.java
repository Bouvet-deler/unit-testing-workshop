package java.Webshop.Service.Payment;

public class ThirdPartyPaymentSystemClient {
    public void waitForConnection() throws InterruptedException {
        // Simulate that we need to connect to a third-party system which is not reachable from non-production environment
        Thread.sleep(365 * 24 * 60 * 60 * 1000L); // Sleep for 365 days
    }

    public boolean handlePayment(String userId, double amountInNok, String message) throws InterruptedException {
        // Simulate some work that may take some time
        Thread.sleep(5 * 1000); // Sleep for 5 seconds
        return true;
    }
}

