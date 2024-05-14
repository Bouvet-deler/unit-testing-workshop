package Webshop.Service.Users;

import java.util.UUID;
import Webshop.Service.Common.PasswordValidator;

interface IUserService {
    User createUser(String email, String firstName, String lastName, String password) throws InvalidPasswordException;
    User getUser(String userId);
}

public class UserService implements IUserService {
    private final IUserRepository userRepository;
    private final PasswordValidator passwordValidator;

    public UserService(UserRepository userRepository, PasswordValidator passwordValidator) {
        this.userRepository = userRepository;
        this.passwordValidator = passwordValidator;
    }

    @Override
    public User createUser(String email, String firstName, String lastName, String password) throws InvalidPasswordException {
        if (!passwordValidator.isValidPassword(password)) {
            throw new InvalidPasswordException("Password does not meet requirements");
        }

        String id = UUID.randomUUID().toString();

        return userRepository.addUser(id, email, firstName, lastName, password);
    }

    @Override
    public User getUser(String userId) {
        return userRepository.getUser(userId);
    }
}

class InvalidPasswordException extends Exception {
    public InvalidPasswordException(String message) {
        super(message);
    }
}

