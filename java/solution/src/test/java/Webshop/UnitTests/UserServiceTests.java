package Webshop.UnitTests;

import org.junit.jupiter.api.Test;
import Webshop.Service.Users.InvalidPasswordException;
import Webshop.Service.Common.PasswordValidator;
import Webshop.Service.Users.UserRepository;
import Webshop.Service.Users.User;
import Webshop.Service.Users.UserService;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class UserServiceTests {
    @Test
    public void createUser_whenPasswordIsInvalid_shouldThrowException() {
        UserService userService = new UserService(mock(UserRepository.class), new PasswordValidator());

        assertThrows(InvalidPasswordException.class, () -> userService.createUser("user1", "valid@email.com", "Test", "User", "12345678"));
    }

    @Test
    public void createUser_whenPasswordIsValid_shouldCreateNewUser() throws InvalidPasswordException {
        UserRepository userRepository = mock(UserRepository.class);
        UserService userService = new UserService(userRepository, new PasswordValidator());

        when(userRepository.addUser("user1","valid@email.com", "Test", "User", "password123")).thenReturn(new User("user1", "valid@email.com", "Test", "User", "password123"));

        User user = userService.createUser("user1","valid@email.com", "Test", "User", "password123");

        assertNotNull(user);
    }

    @Test
    public void createUser_whenUserIsCreated_shouldBeAssignedAnId() throws InvalidPasswordException {
        UserRepository userRepository = mock(UserRepository.class);
        UserService userService = new UserService(userRepository, new PasswordValidator());

        when(userRepository.addUser("user1","valid@email.com", "Test", "User", "password123")).thenReturn(new User("user1", "valid@email.com", "Test", "User", "password123"));

        User user = userService.createUser("user1","valid@email.com", "Test", "User", "password123");

        assertNotNull(user.getId());
    }

    @Test
    public void createUser_whenUserIsCreated_shouldBeRetrievable() throws InvalidPasswordException {
        UserRepository userRepository = mock(UserRepository.class);
        UserService userService = new UserService(userRepository, new PasswordValidator());

        when(userRepository.addUser("user1","valid@email.com", "Test", "User", "password123")).thenReturn(new User("user1", "valid@email.com", "Test", "User", "password123"));
        when(userRepository.getUser("user1")).thenReturn(new User("user1", "valid@email.com", "Test", "User", "password123"));

        User user = userService.createUser("user1","valid@email.com", "Test", "User", "password123");
        User userFromDb = userService.getUser(user.getId());

        assertNotNull(userFromDb);
    }
}

