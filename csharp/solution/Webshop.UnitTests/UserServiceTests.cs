using NUnit.Framework;
using Webshop.Service.Common;
using Webshop.Service.Users;

namespace Webshop.UnitTests;

[TestFixture]
public class UserServiceTests
{
    [Test]
    public void CreateUser_WhenPasswordIsInvalid_ShouldThrowException()
    {
        var userService = new UserService(new UserRepository(), new PasswordValidator());

        // Alternative 1 (manually checking that exception is thrown):
        bool exceptionThrown = false;
        try
        {
            userService.CreateUser("valid@email.com", "Test", "User", "12345678");
        }
        catch (InvalidPasswordException)
        {
            exceptionThrown = true;
        }
        Assert.IsTrue(exceptionThrown);

        // Alternative 2 (using Assert.Throws):
        //Assert.Throws<InvalidPasswordException>(() => userService.CreateUser("valid@email.com", "Test", "User", "12345678"));
    }

    [Test]
    public void CreateUser_WhenPasswordIsValid_ShouldCreateNewUser()
    {
        var userService = new UserService(new UserRepository(), new PasswordValidator());
        var user = userService.CreateUser("valid@email.com", "Test", "User", "password123");

        Assert.That(user, Is.Not.Null);
    }

    [Test]
    public void CreateUser_WhenUserIsCreated_ShouldBeAssignedAnId()
    {
        var userService = new UserService(new UserRepository(), new PasswordValidator());
        var user = userService.CreateUser("valid@email.com", "Test", "User", "password123");

        Assert.That(user.Id, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void CreateUser_WhenUserIsCreated_ShouldBeRetrievable()
    {
        var userService = new UserService(new UserRepository(), new PasswordValidator());

        var user = userService.CreateUser("valid@email.com", "Test", "User", "password123");
        var userFromDb = userService.GetUser(user.Id);

        Assert.That(userFromDb, Is.Not.Null);
    }
}