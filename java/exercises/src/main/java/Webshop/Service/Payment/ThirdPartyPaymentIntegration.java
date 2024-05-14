package Webshop.Service.Payment;

import Webshop.Service.Common.Logger;
import Webshop.Service.Payment.ThirdPartyPaymentSystemClient;

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