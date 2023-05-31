using Microsoft.Extensions.DependencyInjection;

namespace CatchSubscriber.Extentions;

public static class EmailExtension
{
    /// <summary>
    /// Injects <see href="https://github.com/lukencode/FluentEmail">FluentEmail</see> using IServiceCollection
    /// </summary>
    /// <param name="host">SMTP host</param>
    /// <param name="port">SMTP port</param>
    /// <param name="username"><see cref='System.Net.NetworkCredential'>NetworkCredential</see> username</param>
    /// <param name="password"><see cref='System.Net.NetworkCredential'>NetworkCredential</see> password</param>
    /// <param name="defaultFromEmail">FluentEmail default sender</param>
    /// <param name="defaultFromName">FluentEmail default sender name</param>
    public static void AddEmail(this IServiceCollection services, string host, int port, string username, string password, string defaultFromEmail, string defaultFromName = "")
    {
        services
        .AddFluentEmail(defaultFromEmail, defaultFromName)
        .AddSmtpSender(host, port, username, password);
    }
}