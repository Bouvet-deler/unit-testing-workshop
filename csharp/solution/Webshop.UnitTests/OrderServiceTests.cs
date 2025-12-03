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

    [Test]
    public void PlaceOrder_WhenInventoryIsEmpty_ShouldReturnOutOfStock()
    {
        var orderRepository = A.Fake<IOrderRepository>();
        var productService = A.Fake<IProductService>();
        var userService = A.Fake<IUserService>();
        var notificationService = A.Fake<INotificationService>();

        A.CallTo(() => productService.GetProduct("product1")).Returns(new Product { Id = "product1" }); // Make sure the product exists
        A.CallTo(() => productService.GetProductInventory("product1")).Returns(0); // Simulate that inventory is empty

        var orderService = new OrderService(orderRepository, productService, userService, notificationService);

        var orderResult = orderService.PlaceOrder("product1", 1, "user1");

        Assert.That(orderResult.IsSuccess, Is.False, "Expected order to fail");
        Assert.That(orderResult.ResultCode, Is.EqualTo(OrderResultCode.OutOfStock));
    }

    [Test]
    public void PlaceOrder_WhenSuccessful_ShouldUpdateInventory()
    {
        var orderRepository = A.Fake<IOrderRepository>();
        var productService = A.Fake<IProductService>();
        var userService = A.Fake<IUserService>();
        var notificationService = A.Fake<INotificationService>();

        A.CallTo(() => productService.GetProduct("product1")).Returns(new Product { Id = "product1" }); // Make sure the product exists
        A.CallTo(() => productService.GetProductInventory("product1")).Returns(4); // Simulate that inventory has enough items

        var orderService = new OrderService(orderRepository, productService, userService, notificationService);

        var orderResult = orderService.PlaceOrder("product1", 2, "user1");

        Assert.That(orderResult.IsSuccess, Is.True, "Expected order to be successful");
        A.CallTo(() => productService.SubtractFromProductInventory("product1", 2)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void PlaceOrder_WhenSuccessful_ShouldSendNotification()
    {
        var orderRepository = A.Fake<IOrderRepository>();
        var productService = A.Fake<IProductService>();
        var userService = A.Fake<IUserService>();
        var notificationService = A.Fake<INotificationService>();

        A.CallTo(() => productService.GetProduct("product1")).Returns(new Product { Id = "product1" }); // Make sure the product exists
        A.CallTo(() => productService.GetProductInventory("product1")).Returns(1); // Simulate that inventory has enough items

        var orderService = new OrderService(orderRepository, productService, userService, notificationService);

        var orderResult = orderService.PlaceOrder("product1", 1, "user1");

        Assert.That(orderResult.IsSuccess, Is.True, "Expected order to be successful");
        A.CallTo(() => notificationService.SendMail(A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
    }
}