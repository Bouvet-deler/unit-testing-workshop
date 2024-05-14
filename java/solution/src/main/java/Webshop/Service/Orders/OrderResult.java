package Webshop.Service.Orders;

public class OrderResult {
    private final OrderResultCode resultCode;
    private final Order order;

    public OrderResult(OrderResultCode resultCode) {
        this(resultCode, null);
    }

    public OrderResult(OrderResultCode resultCode, Order order) {
        this.resultCode = resultCode;
        this.order = order;
    }

    public boolean isSuccess() {
        return resultCode == OrderResultCode.Successful;
    }

    public OrderResultCode getResultCode() {
        return resultCode;
    }

    public Order getOrder() {
        return order;
    }
}
