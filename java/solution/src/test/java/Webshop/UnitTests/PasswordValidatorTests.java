package Webshop.UnitTests;

import org.junit.jupiter.api.Test;
import Webshop.Service.Common.PasswordValidator;

import static org.junit.jupiter.api.Assertions.*;

public class PasswordValidatorTests {
    @Test
    public void isValidPassword_whenPasswordIsEmpty_shouldReturnFalse() {
        PasswordValidator passwordValidator = new PasswordValidator();
        assertFalse(passwordValidator.isValidPassword(""));
    }

    @Test
    public void isValidPassword_whenPasswordIsTooShort_shouldReturnFalse() {
        PasswordValidator passwordValidator = new PasswordValidator();
        assertFalse(passwordValidator.isValidPassword("pwd123"));
    }

    @Test
    public void isValidPassword_whenPasswordIsMissingCharacters_shouldReturnFalse() {
        PasswordValidator passwordValidator = new PasswordValidator();
        assertFalse(passwordValidator.isValidPassword("12345678"));
    }

    @Test
    public void isValidPassword_whenPasswordIsMissingNumbers_shouldReturnFalse() {
        PasswordValidator passwordValidator = new PasswordValidator();
        assertFalse(passwordValidator.isValidPassword("password"));
    }

    @Test
    public void isValidPassword_whenPasswordIsValid_shouldReturnTrue() {
        PasswordValidator passwordValidator = new PasswordValidator();
        assertTrue(passwordValidator.isValidPassword("password123"));
    }
}


// mvn -Dtest=PasswordValidatorTest test