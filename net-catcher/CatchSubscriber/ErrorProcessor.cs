using Azure.Core.Diagnostics;
using CatchSubscriber.Interfaces;
using CatchSubscriber.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Tracing;

namespace CatchSubscriber;

public class ErrorProcessor : IErrorProcesser
{
    private SlackProcessor SlackProcessor { get; set; }

    public async Task ProcessError(string message, LogLevel logLevel, List<CatchAction>? actions = null)
    {
        await Task.Run(async () =>
        {
            if (actions is null)
            {
                SetColors(logLevel);
                Console.WriteLine(message);
                return;
            }

            foreach (var action in actions)
            {
                switch (action)
                {
                    case CatchAction.Console:
                        SetColors(logLevel);
                        Console.WriteLine(message);
                        break;

                    case CatchAction.Azure:
                        await LogDiagnosticsToAzure(logLevel, message);
                        break;

                    case CatchAction.AWS:
                        break;

                    case CatchAction.Slack:
                        await LogMessageToSlack(logLevel, message);
                        break;

                    default:
                        break;
                }
            }
        });
    }

    public void RegisterSlack(string hookUrl, string channel, string userName, string emoji = "")
    {
        SlackProcessor = RegisterProcessors.InjectSlack(hookUrl, channel, userName, emoji);
    }

    private async Task LogDiagnosticsToAzure(LogLevel logLevel, string message)
    {
        await Task.Run(() =>
        {
            //EventLevel eventLevel = EventLevel.Verbose;

            //switch (logLevel)
            //{
            //    case LogLevel.Error:
            //        eventLevel = EventLevel.Error;
            //        break;

            //    case LogLevel.Critical:
            //        eventLevel = EventLevel.Critical;
            //        break;

            //    case LogLevel.Warning:
            //        eventLevel = EventLevel.Warning;
            //        break;

            //    default:
            //        eventLevel = EventLevel.Verbose;
            //        break;
            //}

            Console.WriteLine($"{logLevel.ToString().ToUpper()} | {DateTime.UtcNow} | {message}");

            //TODO This should be done in the register method/class MOVE THIS
            //try
            //{
            //    using var listener = new AzureEventSourceListener((e, log) =>
            //    {
            //        if (e.EventSource.Name == "Core")//TODO change this
            //        {
            //            Console.WriteLine($"{DateTime.UtcNow} {log}");
            //        }
            //    }, level: eventLevel);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        });
    }

    private async Task LogMessageToSlack(LogLevel logLevel, string message, string channel = "")
    {
        if (SlackProcessor is null)
        {
            throw new ArgumentNullException(nameof(SlackProcessor));
        }

        if (string.IsNullOrEmpty(channel) is false)
        {
            SlackProcessor.SetChannel(channel);
        }

        SlackProcessor.SetMessage(logLevel, message);
        await SlackProcessor.SlackClient.PostAsync(SlackProcessor.SlackMessage);
    }

    private void SetColors(LogLevel LogLevel)
    {
        Console.BackgroundColor = ConsoleColor.White;

        switch (LogLevel)
        {
            case LogLevel.Trace:
                Console.ForegroundColor = ConsoleColor.Black;
                break;

            case LogLevel.Debug:
                Console.ForegroundColor = ConsoleColor.Blue;
                break;

            case LogLevel.Information:
                Console.ForegroundColor = ConsoleColor.Green;
                break;

            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;

            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;

            case LogLevel.Critical:
                Console.ForegroundColor = ConsoleColor.Red;
                break;

            case LogLevel.None:
                break;

            default:
                break;
        }
    }
}