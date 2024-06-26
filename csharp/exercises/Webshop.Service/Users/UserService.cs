﻿using Webshop.Service.Common;

namespace Webshop.Service.Users;

public interface IUserService
{
    User CreateUser(string email, string firstName, string lastName, string password);
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