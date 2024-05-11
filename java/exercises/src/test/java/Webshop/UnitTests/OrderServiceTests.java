package test.java.Webshop.UnitTests;

import org.junit.jupiter.api.Test;

import java.Webshop.Service.Common.NotificationService;
import java.Webshop.Service.Orders.OrderResultCode;
import java.Webshop.Service.Orders.OrderService;
import java.Webshop.Service.Products.ProductService;
import java.Webshop.Service.Users.UserService;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class OrderServiceTests {
    @Test
    public void placeOrder_whenProductIsNotFound_shouldReturnProductNotFound() {
        ProductService productService = mock(ProductService.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);

        when(productService.getProduct("product1")).thenReturn(null); // Simulate that product does not exist

        OrderService orderService = new OrderService(null, productService, userService, notificationService);

        var orderResult = orderService.placeOrder("product1", 1, "user1");

        assertFalse(orderResult.isSuccess(), "Expected order to fail");
        assertEquals(OrderResultCode.ProductNotFound, orderResult.getResultCode());
    }
}
