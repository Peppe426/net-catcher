using Azure.Core.Diagnostics;
using CatchSubscriber.Interfaces;
using CatchSubscriber.Models;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;
using System.Diagnostics.Tracing;

namespace CatchSubscriber;

public class ErrorProcessor : IErrorProcesser
{
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
                        LogDiagnosticsToSlack(logLevel, message);
                        break;

                    default:
                        break;
                }
            }
        });
    }

    private async Task LogDiagnosticsToAzure(LogLevel logLevel, string message)
    {
        await Task.Run(() =>
        {
            EventLevel eventLevel = EventLevel.Verbose;

            switch (logLevel)
            {
                case LogLevel.Error:
                    eventLevel = EventLevel.Error;
                    break;

                case LogLevel.Critical:
                    eventLevel = EventLevel.Critical;
                    break;

                case LogLevel.Warning:
                    eventLevel = EventLevel.Warning;
                    break;

                default:
                    eventLevel = EventLevel.Verbose;
                    break;
            }
            try
            {
                using var listener = new AzureEventSourceListener((e, log) =>
                {
                    if (e.EventSource.Name == "Core")//TODO change this
                    {
                        Console.WriteLine($"{DateTime.UtcNow} {log}");
                    }
                }, level: eventLevel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        });
    }

    private async Task LogDiagnosticsToSlack(LogLevel logLevel, string message)
    {
        string hookUrl = "https://hooks.slack.com/services/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";
        SlackClient slackClient = new(hookUrl);

        var slackMessage = new SlackMessage
        {
            Channel = "#testing",
            Text = message,
            IconEmoji = Emoji.AlarmClock,
            Username = "Tester"
        };

        slackClient.Post(slackMessage);
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