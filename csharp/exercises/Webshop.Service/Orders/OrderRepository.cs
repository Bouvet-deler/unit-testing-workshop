namespace Webshop.Service.Orders;

public interface IOrderRepository
{
    Order AddOrder(string productId, int quantity, string userId);
}

public class OrderRepository : IOrderRepository
{
    private Dictionary<string, Order> _orderDatabase = new();

    public Order AddOrder(string productId, int quantity, string userId)
    {
        var order = new Order { OrderId = Guid.NewGuid().ToString(), ProductId = productId, Quantity = quantity, UserId = userId };
        _orderDatabase.Add(order.OrderId, order);
        return order;
    }
}

public class Order
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public string UserId { get; set; }
}