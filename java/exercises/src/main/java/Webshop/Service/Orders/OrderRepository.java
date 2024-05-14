package Webshop.Service.Orders;

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

