using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CatchSubscriber.Guards;

public static class EmailGuard
{
    /// <summary>
    /// Validate email and returns email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns></returns>
    /// <exception cref="ValidationException">Throws "Email is not valid"</exception>
    public static string IsEmail(this string email)
    {
        var output = new EmailAddressAttribute().IsValid(email);
        if (output is false && IsValidRegexEmail(email) is false)
        {
            throw new ValidationException("Email is not valid");
        }

        return email;
    }

    private static bool IsValidRegexEmail(string email)
    {
        var regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        return regex.IsMatch(email);
    }
}