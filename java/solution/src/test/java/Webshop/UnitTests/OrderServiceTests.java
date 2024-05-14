package Webshop.UnitTests;

import Webshop.Service.Orders.OrderRepository;
import Webshop.Service.Orders.OrderResult;
import Webshop.Service.Users.User;
import org.junit.jupiter.api.Test;
import Webshop.Service.Common.NotificationService;
import Webshop.Service.Orders.OrderResultCode;
import Webshop.Service.Orders.OrderService;
import Webshop.Service.Products.ProductService;
import Webshop.Service.Products.Product;
import Webshop.Service.Users.UserService;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class OrderServiceTests {
    @Test
    public void placeOrder_whenProductIsNotFound_shouldReturnProductNotFound() {
        ProductService productService = mock(ProductService.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);
        OrderRepository orderRepository = mock(OrderRepository.class);

        when(productService.getProduct("product1")).thenReturn(null); // Simulate that product does not exist

        OrderService orderService = new OrderService(orderRepository, productService, userService, notificationService);

        OrderResult orderResult = orderService.placeOrder("product1", 1, "user1");

        assertFalse(orderResult.isSuccess(), "Expected order to fail");
        assertEquals(OrderResultCode.ProductNotFound, orderResult.getResultCode());
    }

    @Test
    public void placeOrder_whenInventoryIsEmpty_shouldReturnOutOfStock() {
        ProductService productService = mock(ProductService.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);
        OrderRepository orderRepository = mock(OrderRepository.class);

        when(productService.getProduct("product1")).thenReturn(new Product("product1", 10)); // Make sure the product exists
        when(productService.getProductInventory("product1")).thenReturn(0); // Simulate that inventory is empty

        OrderService orderService = new OrderService(orderRepository, productService, userService, notificationService);

        OrderResult orderResult = orderService.placeOrder("product1", 1, "user1");

        assertFalse(orderResult.isSuccess(), "Expected order to fail");
        assertEquals(OrderResultCode.OutOfStock, orderResult.getResultCode());
    }

    @Test
    public void placeOrder_whenSuccessful_shouldUpdateInventory() {
        ProductService productService = mock(ProductService.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);
        OrderRepository orderRepository = mock(OrderRepository.class);

        when(userService.getUser("user1")).thenReturn(new User("user1", "a", "b", "c", "Abcd1234")); // Make sure the user exists
        when(productService.getProduct("product1")).thenReturn(new Product("product1", 10)); // Make sure the product exists
        when(productService.getProductInventory("product1")).thenReturn(4); // Simulate that inventory has enough items

        OrderService orderService = new OrderService(orderRepository, productService, userService, notificationService);

        OrderResult orderResult = orderService.placeOrder("product1", 2, "user1");

        assertTrue(orderResult.isSuccess(), "Expected order to be successful");
        verify(productService, times(1)).subtractFromProductInventory("product1", 2);
    }

    @Test
    public void placeOrder_whenSuccessful_shouldSendNotification() {
        ProductService productService = mock(ProductService.class);
        UserService userService = mock(UserService.class);
        NotificationService notificationService = mock(NotificationService.class);
        OrderRepository orderRepository = mock(OrderRepository.class);

        when(userService.getUser("user1")).thenReturn(new User("user1", "a", "b", "c", "Abcd1234")); // Make sure the user exists
        when(productService.getProduct("product1")).thenReturn(new Product("product1", 1)); // Make sure the product exists
        when(productService.getProductInventory("product1")).thenReturn(1); // Simulate that inventory has enough items

        OrderService orderService = new OrderService(orderRepository, productService, userService, notificationService);

        OrderResult orderResult = orderService.placeOrder("product1", 1, "user1");

        assertTrue(orderResult.isSuccess(), "Expected order to be successful");
        verify(notificationService, times(1)).sendMail(anyString(), anyString());
    }
}


// Run the test with the following command:
// mvn -Dtest=OrderServiceTests test

// Run a single test with the following command:
// mvn -Dtest=OrderServiceTests#placeOrder_whenProductIsNotFound_shouldReturnProductNotFound test