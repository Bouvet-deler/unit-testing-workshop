namespace Webshop.Service.Users;

public interface IUserRepository
{
    User AddUser(string id, string email, string firstName, string lastName, string password);
    User? GetUser(string userId);
}

public class UserRepository : IUserRepository
{
    private readonly Dictionary<string, User> _userDatabase = new();

    public User AddUser(string id, string email, string firstName, string lastName, string password)
    {
        var user = new User
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Password = password
        };
        _userDatabase.Add(user.Id, user);
        // This repository will use a real database, a really slow database
        Thread.Sleep(TimeSpan.FromSeconds(10));
        return user;
    }

    public User? GetUser(string userId)
    {
        // This repository will use a real database, a really slow database
        Thread.Sleep(TimeSpan.FromSeconds(10));
        if (_userDatabase.TryGetValue(userId, out var user))
            return user;
        return null;
    }
}

public class User
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}