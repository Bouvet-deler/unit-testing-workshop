namespace Webshop.Service;

public interface IOrderService
{
    OrderResult PlaceOrder(string productId, int quantity, string userId);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductService _productService;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public OrderService(IOrderRepository orderRepository, IProductService productService, IUserService userService, INotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _productService = productService;
        _userService = userService;
        _notificationService = notificationService;
    }

    public OrderResult PlaceOrder(string productId, int quantity, string userId)
    {
        // Verify input arguments
        if (string.IsNullOrEmpty(productId))
            throw new ArgumentNullException(nameof(productId), "Product id must be specified");
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero");
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId), "User id must be specified");

        // Get product-info with price
        var product = _productService.GetProduct(productId);
        if (product == null)
            return new OrderResult { ResultCode = OrderResultCode.ProductNotFound };

        // Get inventory-info and verify that we have enough items of the specified product
        var inventory = _productService.GetProductInventory(productId);
        if (inventory < quantity)
            return new OrderResult { ResultCode = OrderResultCode.OutOfStock };

        // Handle payment
        var paymentAmount = product.Price * quantity;
        // PaymentService is not yet very testable - we will fix this in the last part of the workshop
        //_paymentService.HandlePayment(userId, paymentAmount);

        // Update inventory
        _productService.SubtractFromProductInventory(productId, quantity);

        // Send notification
        var user = _userService.GetUser(userId);
        _notificationService.SendMail(user.Email, $"Order confirmation: Your order for product {productId} has been succesful.");

        // Create and return order
        var order = _orderRepository.AddOrder(productId, quantity, userId);
        return new OrderResult { Order = order, ResultCode = OrderResultCode.Successful };
    }
}

public class OrderResult
{
    public bool IsSuccess => ResultCode == OrderResultCode.Successful;
    public OrderResultCode ResultCode { get; set; }
    public Order? Order { get; set; }
}

public enum OrderResultCode
{
    Successful = 0,
    ProductNotFound = 1,
    OutOfStock = 2,
    PaymentFailed = 3
}