package Webshop.UnitTests;

import Webshop.Service.Common.Logger;
import Webshop.Service.Users.User;
import org.junit.jupiter.api.Test;
import Webshop.Service.Common.NotificationService;
import Webshop.Service.Payment.ThirdPartyPaymentIntegration;
import Webshop.Service.Payment.PaymentResult;
import Webshop.Service.Payment.PaymentResultCode;
import Webshop.Service.Payment.PaymentService;
import Webshop.Service.Users.UserService;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class PaymentServiceTests {
    // Problem 1: Cannot fake the third party payment service
    // Problem 2: Cannot simulate the user exist or does not exist
    // Problem 3: The logger will try to log to a file on a server somewhere
    // Problem 4: No way to verify that email has been sent
    // Problem 5: No way to verify the result

    @Test
    public void handlePayment_whenNegativeAmount_shouldFail() {
        ThirdPartyPaymentIntegration thirdPartyPaymentIntegration = mock(ThirdPartyPaymentIntegration.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);
        Logger logger = mock(Logger.class);

        PaymentService paymentService = new PaymentService(thirdPartyPaymentIntegration, userService, notificationService, logger);

        when(userService.getUser("user1")).thenReturn(new User("user1", "a", "b", "c", "Abcd1234")); // Make sure the user exists

        PaymentResult result = paymentService.handlePayment("user1", -1);

        assertFalse(result.isSuccess());
        assertEquals(PaymentResultCode.INVALID_AMOUNT, result.getCode());
    }

    @Test
    public void handlePayment_whenUserDoesNotExist_shouldFail() {
        ThirdPartyPaymentIntegration thirdPartyPaymentIntegration = mock(ThirdPartyPaymentIntegration.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);
        Logger logger = mock(Logger.class);

        when(userService.getUser("user2")).thenReturn(null);

        PaymentService paymentService = new PaymentService(thirdPartyPaymentIntegration, userService, notificationService, logger);

        PaymentResult result = paymentService.handlePayment("user2", 123);

        assertFalse(result.isSuccess());
        assertEquals(PaymentResultCode.USER_NOT_FOUND, result.getCode());
    }

    @Test
    public void handlePayment_whenPaymentIsSuccessful_shouldSendEmailToUser() {
        ThirdPartyPaymentIntegration thirdPartyPaymentIntegration = mock(ThirdPartyPaymentIntegration.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);
        Logger logger = mock(Logger.class);

        when(thirdPartyPaymentIntegration.handlePaymentForUser(anyString(), anyDouble(), anyString())).thenReturn(true);
        when(userService.getUser("user1")).thenReturn(new User("user1", "a", "b", "c", "Abcd1234")); // Make sure the user exists

        PaymentService paymentService = new PaymentService(thirdPartyPaymentIntegration, userService, notificationService, logger);

        PaymentResult result = paymentService.handlePayment("user1", 123);

        assertTrue(result.isSuccess(), result.getErrorMessage());
        verify(notificationService, times(1)).sendMail(anyString(), anyString());
    }
}

// mvn -Dtest=PaymentServiceTests test