package Webshop.UnitTests;

import Webshop.Service.Common.PasswordValidator;
import org.junit.jupiter.api.Test;

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

// mvn -Dtest=PasswordValidatorTests#myFirstTest test