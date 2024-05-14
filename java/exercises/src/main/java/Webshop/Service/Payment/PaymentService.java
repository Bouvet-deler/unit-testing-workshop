package Webshop.Service.Payment;

import Webshop.Service.Common.Logger;
import Webshop.Service.Common.NotificationService;
import Webshop.Service.Users.UserRepository;
import Webshop.Service.Users.UserService;
import Webshop.Service.Common.PasswordValidator;
import Webshop.Service.Users.UserRepository;
import Webshop.Service.Users.User;

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
        User user = userService.getUser(userId);
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

