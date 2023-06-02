using CatchSubscriber.Models;
using Microsoft.Extensions.Logging;

namespace CatchSubscriber.Interfaces;

public interface IErrorProcesser
{
    Task ProcessError(string message, LogLevel logLevel, params CatchAction[] actions);

    ErrorProcessor RegisterSlack(string applicationName, string version, string hookUrl, string channel, string userName, string emoji = "");

    //ErrorProcessor RegisterEmail(string applicationName, string version, string fromEmail, string toEmail, string fromName = "", string toName = "");
}