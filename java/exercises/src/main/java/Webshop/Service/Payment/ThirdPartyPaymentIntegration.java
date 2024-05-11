package java.Webshop.Service.Payment;

import java.Webshop.Service.Common.Logger;

public class ThirdPartyPaymentIntegration {
    private final ThirdPartyPaymentSystemClient client;

    public ThirdPartyPaymentIntegration() {
        this.client = new ThirdPartyPaymentSystemClient();
    }

    public boolean handlePaymentForUser(String userId, double amountInNok, String message) {
        try {
            client.waitForConnection();
            return client.handlePayment(userId, amountInNok, message);
        } catch (Exception ex) {
            Logger.logError("Payment failed: " + ex.getMessage());
            return false;
        }
    }
}