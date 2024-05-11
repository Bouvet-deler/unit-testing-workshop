package java.Webshop.Service.Common;

import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;


public class PasswordValidator {
    // Minimum eight characters, at least one letter and one number
    private final Pattern passwordPattern = Pattern.compile("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$");

    public boolean isValidPassword(String password) {
        Matcher matcher = passwordPattern.matcher(password);
        return matcher.matches();
    }

    public static void main(String[] args) {
        PasswordValidator passwordValidator = new PasswordValidator();
        Scanner scanner = new Scanner(System.in);

        System.out.print("Enter a password to validate: ");
        String password = scanner.nextLine();

        boolean isValid = passwordValidator.isValidPassword(password);
        System.out.println("Is valid password? " + isValid);

        scanner.close();
    }
}
