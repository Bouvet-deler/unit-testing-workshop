namespace Webshop.Service;

public interface IUserService
{
    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <param name="password">Password</param>
    /// <returns>The created user with a generated unique id</returns>
    /// <exception cref="InvalidPasswordException">Thrown if password does not meet requirements</exception>
    User CreateUser(string email, string firstName, string lastName, string password);

    /// <summary>
    /// Returns user with the specified id
    /// </summary>
    /// <param name="userId">The id of the user to return</param>
    /// <returns>User with specified id, or NULL if not found</returns>
    User? GetUser(string userId);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordValidator _passwordValidator;

    public UserService(IUserRepository userRepository, IPasswordValidator passwordValidator)
    {
        _userRepository = userRepository;
        _passwordValidator = passwordValidator;
    }

    public User CreateUser(string email, string firstName, string lastName, string password)
    {
        if (!_passwordValidator.IsValidPassword(password))
            throw new InvalidPasswordException("Password does not meet requirements");

        var id = Guid.NewGuid().ToString();

        return _userRepository.AddUser(id, email, firstName, lastName, password);
    }

    public User? GetUser(string userId)
    {
        return _userRepository.GetUser(userId);
    }
}

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException(string message) : base(message) { }
}