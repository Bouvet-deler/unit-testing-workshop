package java.Webshop.Service.Orders;

import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

interface IOrderRepository {
    Order addOrder(String productId, int quantity, String userId);
}

public class OrderRepository implements IOrderRepository {
    private final Map<String, Order> orderDatabase = new HashMap<>();

    @Override
    public Order addOrder(String productId, int quantity, String userId) {
        Order order = new Order(UUID.randomUUID().toString(), productId, quantity, userId);
        orderDatabase.put(order.getOrderId(), order);
        return order;
    }
}

class Order {
    private String orderId;
    private String productId;
    private int quantity;
    private String userId;

    public Order(String orderId, String productId, int quantity, String userId) {
        this.orderId = orderId;
        this.productId = productId;
        this.quantity = quantity;
        this.userId = userId;
    }

    public String getOrderId() {
        return orderId;
    }

    public String getProductId() {
        return productId;
    }

    public int getQuantity() {
        return quantity;
    }

    public String getUserId() {
        return userId;
    }
}

