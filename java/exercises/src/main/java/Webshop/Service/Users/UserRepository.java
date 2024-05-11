package java.Webshop.Service.Users;

import java.util.HashMap;
import java.util.Map;

interface IUserRepository {
    User addUser(String id, String email, String firstName, String lastName, String password);
    User getUser(String userId);
}

public class UserRepository implements IUserRepository {
    private final Map<String, User> userDatabase = new HashMap<>();

    @Override
    public User addUser(String id, String email, String firstName, String lastName, String password) {
        User user = new User(id, email, firstName, lastName, password);
        userDatabase.put(user.getId(), user);
        // This repository will use a real database, a really slow database
        try {
            Thread.sleep(10 * 1000); // Sleep for 10 seconds
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return user;
    }

    @Override
    public User getUser(String userId) {
        // This repository will use a real database, a really slow database
        try {
            Thread.sleep(10 * 1000); // Sleep for 10 seconds
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return userDatabase.get(userId);
    }
}

