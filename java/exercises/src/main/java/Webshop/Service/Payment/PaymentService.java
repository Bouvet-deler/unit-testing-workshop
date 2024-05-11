package java.Webshop.Service.Payment;

import java.Webshop.Service.Common.Logger;
import java.Webshop.Service.Common.NotificationService;
import java.Webshop.Service.Users.UserRepository;
import java.Webshop.Service.Users.UserService;

interface IPaymentService {
    void handlePayment(String userId, double amountInNok);
}

public class PaymentService implements IPaymentService {
    private final ThirdPartyPaymentIntegration thirdPartyPaymentIntegration;

    public PaymentService() {
        this.thirdPartyPaymentIntegration = new ThirdPartyPaymentIntegration();
    }

    @Override
    public void handlePayment(String userId, double amountInNok) {
        UserService userService = new UserService(new UserRepository(), new PasswordValidator());
        UserRepository user = userService.getUser(userId);
        if (user == null) {
            Logger.logError("Attempted to handle payment for non-existing user " + userId);
            throw new RuntimeException("Could not find user with id " + userId);
        }

        if (amountInNok < 0) {
            throw new IllegalArgumentException("Amount cannot be negative");
        }

        boolean paymentOk = thirdPartyPaymentIntegration.handlePaymentForUser(userId, amountInNok, "WebShop payment");
        if (!paymentOk) {
            Logger.logError("Payment failed for user " + userId + " with amount " + amountInNok);
            throw new RuntimeException("Payment failed");
        }

        NotificationService notificationService = new NotificationService();
        notificationService.sendMail(user.getEmail(), "Payment succeeded");
    }
}

