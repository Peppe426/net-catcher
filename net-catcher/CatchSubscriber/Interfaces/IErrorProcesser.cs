using CatchSubscriber.Models;
using Microsoft.Extensions.Logging;

namespace CatchSubscriber.Interfaces;

public interface IErrorProcesser
{
    Task ProcessError(string message, LogLevel logLevel, List<CatchAction>? actions = null, LogLevel level = LogLevel.Critical);

    ErrorProcessor RegisterSlack(string hookUrl, string channel, string userName, string emoji = "");
}