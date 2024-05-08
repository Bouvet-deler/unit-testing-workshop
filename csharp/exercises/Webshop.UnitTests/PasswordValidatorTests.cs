using NUnit.Framework;
using Webshop.Service;

namespace Webshop.UnitTests;

[TestFixture]
public class PasswordValidatorTests
{
    [Test]
    public void MyFirstTest()
    {
        // Arrange
        var passwordValidator = new PasswordValidator();
        // Act
        var isValid = passwordValidator.IsValidPassword("");
        // Assert
        // TODO: Write your assertions here
    }
}