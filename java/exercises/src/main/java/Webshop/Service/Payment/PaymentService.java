package Webshop.Service.Payment;

import Webshop.Service.Common.Logger;
import Webshop.Service.Common.NotificationService;
import Webshop.Service.Payment.PaymentResult;
import Webshop.Service.Payment.PaymentResultCode.*;
import Webshop.Service.Users.UserService;
import Webshop.Service.Users.User;

//import static Webshop.Service.Common.Logger.logError;

interface IPaymentService {
    PaymentResult handlePayment(String userId, double amountInNok);
}

public class PaymentService implements IPaymentService {
    private final ThirdPartyPaymentIntegration thirdPartyPaymentIntegration;
    private final UserService userService;
    private final NotificationService notificationService;
    private final Logger logger;

    public PaymentService(ThirdPartyPaymentIntegration thirdPartyPaymentIntegration, UserService userService, NotificationService notificationService, Logger logger) {
        this.thirdPartyPaymentIntegration = thirdPartyPaymentIntegration;
        this.userService = userService;
        this.notificationService = notificationService;
        this.logger = logger;
    }

    public PaymentResult handlePayment(String userId, double amountInNok) {
        User user = userService.getUser(userId);
        if (user == null) {
            String msg = String.format("Attempted to handle payment for non-existing user %s", userId);
            logger.logError(msg);
            return new PaymentResult(PaymentResultCode.USER_NOT_FOUND, msg);
        }

        if (amountInNok < 0) {
            return new PaymentResult(PaymentResultCode.INVALID_AMOUNT, "Amount cannot be negative");
        }

        boolean paymentOk = thirdPartyPaymentIntegration.handlePaymentForUser(userId, amountInNok, "WebShop payment");
        if (!paymentOk) {
            String msg = String.format("Payment failed for user %s with amount %.2f", userId, amountInNok);
            logger.logError(msg);
            return new PaymentResult(PaymentResultCode.PAYMENT_INTEGRATION_FAILED, msg);
        }

        notificationService.sendMail(user.getEmail(), "Payment succeeded");

        return new PaymentResult(PaymentResultCode.SUCCESS);
    }
}

