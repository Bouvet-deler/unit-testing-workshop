using FakeItEasy;
using NUnit.Framework;
using Webshop.Service.Common;
using Webshop.Service.Orders;
using Webshop.Service.Products;
using Webshop.Service.Users;

namespace Webshop.UnitTests;

[TestFixture]
public class OrderServiceTests
{
    [Test]
    public void PlaceOrder_WhenProductIsNotFound_ShouldReturnProductNotFound()
    {
        var orderRepository = A.Fake<IOrderRepository>();
        var productService = A.Fake<IProductService>();
        var userService = A.Fake<IUserService>();
        var notificationService = A.Fake<INotificationService>();

        A.CallTo(() => productService.GetProduct("product1")).Returns(null); // Simulate that product does not exist

        var orderService = new OrderService(orderRepository, productService, userService, notificationService);

        var orderResult = orderService.PlaceOrder("product1", 1, "user1");

        Assert.That(orderResult.IsSuccess, Is.False, "Expected order to fail");
        Assert.That(orderResult.ResultCode, Is.EqualTo(OrderResultCode.ProductNotFound));
    }
}