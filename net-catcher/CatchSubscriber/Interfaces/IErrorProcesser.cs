using CatchSubscriber.Models;
using Microsoft.Extensions.Logging;

namespace CatchSubscriber.Interfaces;

internal interface IErrorProcesser
{
    Task ProcessError(string message, LogLevel logLevel, List<CatchAction>? actions = null);

    void RegisterSlack(string hookUrl, string channel, string userName, string emoji = "");
}