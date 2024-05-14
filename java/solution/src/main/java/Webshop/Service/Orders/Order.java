package Webshop.Service.Orders;

public class Order {
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
