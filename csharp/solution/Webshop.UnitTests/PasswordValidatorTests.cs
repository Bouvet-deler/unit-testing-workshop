using NUnit.Framework;
using Webshop.Service.Common;

namespace Webshop.UnitTests;

[TestFixture]
public class PasswordValidatorTests
{
    [Test]
    public void IsValidPassword_WhenPasswordIsEmpty_ShouldReturnFalse()
    {
        var passwordValidator = new PasswordValidator();
        var isValid = passwordValidator.IsValidPassword("");
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void IsValidPassword_WhenPasswordIsTooShort_ShouldReturnFalse()
    {
        var passwordValidator = new PasswordValidator();
        var isValid = passwordValidator.IsValidPassword("pwd123");
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void IsValidPassword_WhenPasswordIsMissingCharacters_ShouldReturnFalse()
    {
        var passwordValidator = new PasswordValidator();
        var isValid = passwordValidator.IsValidPassword("12345678");
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void IsValidPassword_WhenPasswordIsMissingNumbers_ShouldReturnFalse()
    {
        var passwordValidator = new PasswordValidator();
        var isValid = passwordValidator.IsValidPassword("password");
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void IsValidPassword_WhenPasswordIsValid_ShouldReturnTrue()
    {
        var passwordValidator = new PasswordValidator();
        var isValid = passwordValidator.IsValidPassword("password123");
        Assert.That(isValid, Is.True);
    }
}