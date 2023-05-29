using CatchSubscriber.Interfaces;
using CatchSubscriber.Models;
using Microsoft.Extensions.Logging;

namespace CatchSubscriber;

public class ErrorProcessor : IErrorProcesser
{
    private ProcessorHandler RegistratedProcessors { get; set; } = new ProcessorHandler();

    /// <summary>
    /// Register slack using provided webhook
    /// <see href="https://slack.com/services/new/incoming-webhook">Incoming WebHook</see>
    /// </summary>
    /// <param name="hookUrl"><see href="https://slack.com/services/new/incoming-webhook">Incoming WebHook</see></param>
    /// <param name="channel">Slack channel</param>
    /// <param name="userName">Name of the sender in slack</param>
    /// <param name="emoji">nullable, if not provided the Slack.Webhooks.Emoji.AlarmClock will be used</param>
    /// <returns><see cref="ErrorProcessor">ErrorProcessor</see></returns>
    public ErrorProcessor RegisterSlack(string hookUrl, string channel, string userName, string emoji = "")
    {
        RegistratedProcessors = RegistratedProcessors.InjectSlack(hookUrl, channel, userName, emoji);
        return this;
    }

    /// <summary>
    /// Process error using provided actions <see cref="CatchAction"/>
    /// </summary>
    /// <param name="actions"><see cref="CatchAction"/> If no action is provided only console log will be used</param>
    /// <param name="level">level on when to take action, default is <see cref="LogLevel.Critical"/></param>
    /// <returns></returns>
    public async Task ProcessError(string message, LogLevel logLevel, List<CatchAction>? actions = null, LogLevel level = LogLevel.Critical)
    {
        await Task.Run(async () =>
        {
            if (actions is null)
            {
                Console.WriteLine(message);
                return;
            }

            foreach (var action in actions)
            {
                switch (action)
                {
                    case CatchAction.Console:
                        await LogMessageToConsole(logLevel, message, level);
                        break;

                    case CatchAction.Azure:
                        await LogDiagnosticsToAzure(logLevel, message, level);
                        break;

                    case CatchAction.AWS:
                        await LogMessageToAws(logLevel, message, level);
                        break;

                    case CatchAction.Slack:
                        await LogMessageToSlack(logLevel, message);
                        break;

                    case CatchAction.Email:
                        await LogMessageToEmail(logLevel, message, level);
                        break;

                    default:
                        break;
                }
            }
        });
    }

    private async Task LogMessageToConsole(LogLevel logLevel, string message, LogLevel level)
    {
        if (logLevel == level)
        {
            SetColors(logLevel);
            await LogMessage(logLevel, message);
        }
    }

    private async Task LogDiagnosticsToAzure(LogLevel logLevel, string message, LogLevel level)
    {
        if (logLevel == level)
        {
            await LogMessage(logLevel, message);
        }
    }

    private async Task LogMessageToAws(LogLevel logLevel, string message, LogLevel level)
    {
        if (logLevel == level)
        {
            await LogMessage(logLevel, message);
        }
    }

    private async Task LogMessageToSlack(LogLevel logLevel, string message, string channel = "")
    {
        if (RegistratedProcessors.Slack is null)
        {
            throw new ArgumentNullException(nameof(Processors.SlackProcessor));
        }

        if (string.IsNullOrEmpty(channel) is false)
        {
            RegistratedProcessors.Slack.SetChannel(channel);
        }

        RegistratedProcessors.Slack.SetMessage(logLevel, message);
        await RegistratedProcessors.Slack.SlackClient.PostAsync(RegistratedProcessors.Slack.SlackMessage);
    }

    /// <summary>
    /// Sends an email when level of the loglevel is meet
    /// </summary>
    /// <param name="level">set level on when to log using email</param>
    private async Task LogMessageToEmail(LogLevel logLevel, string message, LogLevel level)
    {
        if (logLevel == level)
        {
            //TODO add logic using FluentEmail
        }
    }

    private async Task LogMessage(LogLevel logLevel, string message)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"{logLevel.ToString().ToUpper()} | {DateTime.UtcNow} | {message}");
        });
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