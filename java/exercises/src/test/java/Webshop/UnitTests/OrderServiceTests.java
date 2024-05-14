package Webshop.UnitTests;

import Webshop.Service.Orders.OrderRepository;
import org.junit.jupiter.api.Test;

import Webshop.Service.Common.NotificationService;
import Webshop.Service.Orders.OrderResultCode;
import Webshop.Service.Orders.OrderService;
import Webshop.Service.Products.ProductService;
import Webshop.Service.Users.UserService;
import Webshop.Service.Orders.OrderResult;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class OrderServiceTests {
    @Test
    public void placeOrder_whenProductIsNotFound_shouldReturnProductNotFound() {
        ProductService productService = mock(ProductService.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);

        when(productService.getProduct("product1")).thenReturn(null); // Simulate that product does not exist

        OrderService orderService = new OrderService(new OrderRepository(), productService, userService, notificationService);

        OrderResult orderResult = orderService.placeOrder("product1", 1, "user1");

        assertFalse(orderResult.isSuccess(), "Expected order to fail");
        assertEquals(OrderResultCode.ProductNotFound, orderResult.getResultCode());
    }
}

// Run the test with the following command:
// mvn -Dtest=OrderServiceTests test