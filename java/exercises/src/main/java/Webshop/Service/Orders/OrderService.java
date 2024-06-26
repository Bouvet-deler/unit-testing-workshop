package Webshop.Service.Orders;

import Webshop.Service.Common.NotificationService;
import Webshop.Service.Products.ProductService;
import Webshop.Service.Users.User;
import Webshop.Service.Users.UserService;
import Webshop.Service.Products.Product;
import Webshop.Service.Orders.OrderResultCode;
import Webshop.Service.Orders.OrderResult;

import java.util.Objects;

interface IOrderService {
    OrderResult placeOrder(String productId, int quantity, String userId);
}

public class OrderService implements IOrderService {
    private final OrderRepository orderRepository;
    private final ProductService productService;
    private final UserService userService;
    private final NotificationService notificationService;

    public OrderService(OrderRepository orderRepository, ProductService productService, UserService userService, NotificationService notificationService) {
        this.orderRepository = Objects.requireNonNull(orderRepository, "Order repository must not be null");
        this.productService = Objects.requireNonNull(productService, "Product service must not be null");
        this.userService = Objects.requireNonNull(userService, "User service must not be null");
        this.notificationService = Objects.requireNonNull(notificationService, "Notification service must not be null");

    }

    @Override
    public OrderResult placeOrder(String productId, int quantity, String userId) {
        // Verify input arguments
        if (productId == null || productId.isEmpty()) {
            throw new IllegalArgumentException("Product id must be specified");
        }
        if (quantity <= 0) {
            throw new IllegalArgumentException("Quantity must be greater than zero");
        }
        if (userId == null || userId.isEmpty()) {
            throw new IllegalArgumentException("User id must be specified");
        }

        // Get product-info with price
        Product product = productService.getProduct(productId);
        if (product == null) {
            return new OrderResult(OrderResultCode.ProductNotFound);
        }

        // Get inventory-info and verify that we have enough items of the specified product
        int inventory = productService.getProductInventory(productId);
        if (inventory < quantity) {
            return new OrderResult(OrderResultCode.OutOfStock);
        }

        // Handle payment
        double paymentAmount = product.getPrice() * quantity;
        // PaymentService is not yet very testable - we will fix this in the last part of the workshop
//        paymentService.handlePayment(userId, paymentAmount);

        // Update inventory
        productService.subtractFromProductInventory(productId, quantity);

        // Send notification
        User user = userService.getUser(userId);
        notificationService.sendMail(user.getEmail(), "Order confirmation: Your order for product " + productId + " has been successful.");

        // Create and return order
        Order order = orderRepository.addOrder(productId, quantity, userId);
        return new OrderResult(OrderResultCode.Successful, order);
    }
}