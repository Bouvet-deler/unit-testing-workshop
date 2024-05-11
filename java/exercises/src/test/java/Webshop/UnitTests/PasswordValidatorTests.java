package test.java.Webshop.UnitTests;

import java.Webshop.Service.Common.PasswordValidator;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class PasswordValidatorTests {

    @Test
    public void myFirstTest() {
        // Arrange
        PasswordValidator passwordValidator = new PasswordValidator();

        // Act
        boolean isValid = passwordValidator.isValidPassword("");

        // Assert
        // TODO: Write your assertions here
    }
}