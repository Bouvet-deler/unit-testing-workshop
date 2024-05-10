using FakeItEasy;
using NUnit.Framework;
using Webshop.Service;

namespace Webshop.UnitTests;

[TestFixture]
public class PaymentServiceTests
{
    // Problem 1: Cannot fake the third party payment service
    // Problem 2: Cannot simulate the user exist or does not exist
    // Problem 3: The logger will try to log to a file on a server somewhere
    // Problem 4: No way to verify that email has been sent
    // Problem 5: No way to verify the result

    [Test]
    public void HandlePayment_WhenNegativeAmount_ShouldFail()
    {
        var thirdPartyPaymentIntegration = A.Fake<IThirdPartyPaymentIntegration>();
        var userService = A.Fake<IUserService>();
        var notificationService = A.Fake<INotificationService>();

        var paymentService = new PaymentService(thirdPartyPaymentIntegration, userService, notificationService, null);

        var result = paymentService.HandlePayment("user1", -1);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Code, Is.EqualTo(PaymentResultCode.InvalidAmount));
    }

    [Test]
    public void HandlePayment_WhenUserDoesNotExist_ShouldFail()
    {
        var thirdPartyPaymentIntegration = A.Fake<IThirdPartyPaymentIntegration>();
        var userService = A.Fake<IUserService>();
        var notificationService = A.Fake<INotificationService>();

        A.CallTo(() => userService.GetUser("user2")).Returns(null);

        var paymentService = new PaymentService(thirdPartyPaymentIntegration, userService, notificationService, null);

        var result = paymentService.HandlePayment("user2", 123);

        Assert.That(result.Success, Is.False);
        Assert.That(result.Code, Is.EqualTo(PaymentResultCode.UserNotFound));
    }

    [Test]
    public void HandlePayment_WhenPaymentIsSuccessful_ShouldSendEmailToUser()
    {
        var thirdPartyPaymentIntegration = A.Fake<IThirdPartyPaymentIntegration>();
        var userService = A.Fake<IUserService>();
        var notificationService = A.Fake<INotificationService>();

        A.CallTo(() => thirdPartyPaymentIntegration.HandlePaymentForUser(A<string>.Ignored, A<decimal>.Ignored, A<string>.Ignored)).Returns(true);

        var paymentService = new PaymentService(thirdPartyPaymentIntegration, userService, notificationService, null);

        var result = paymentService.HandlePayment("user1", 123);

        Assert.That(result.Success, Is.True, result.ErrorMessage);
        A.CallTo(() => notificationService.SendMail(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }
}