using System.Text.RegularExpressions;

namespace Webshop.Service;

public interface IPasswordValidator
{
    bool IsValidPassword(string password);
}

public class PasswordValidator : IPasswordValidator
{
    // Minimum eight characters, at least one letter and one number
    private readonly Regex _passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");

    public bool IsValidPassword(string password)
    {
        return _passwordRegex.IsMatch(password);
    }
}