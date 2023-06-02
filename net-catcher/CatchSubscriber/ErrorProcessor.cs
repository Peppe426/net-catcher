using CatchSubscriber.Interfaces;
using CatchSubscriber.Models;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace CatchSubscriber;

public class ErrorProcessor : IErrorProcesser
{
    private IFluentEmail _fluentEmail;

    public ErrorProcessor(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }
    private ErrorProcessor()
    {

    }

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
    public ErrorProcessor RegisterSlack(string applicationName, string version, string hookUrl, string channel, string userName, string emoji = "")
    {
        RegistratedProcessors = RegistratedProcessors.InjectSlack(hookUrl, channel, userName, emoji);
        return this;
    }

    public ErrorProcessor RegisterEmail(string applicationName, string version, string toEmail, string toName = "", List<(string emailAddress, string? name)> copies = null)
    {
        RegistratedProcessors.SetApplicationName(applicationName);
        RegistratedProcessors.SetVersion(version);

        RegistratedProcessors.InjectEmail(toEmail, toName, copies);
        return this;
    }

    /// <summary>
    /// Process error using provided actions <see cref="CatchAction"/>
    /// </summary>
    /// <param name="actions"><see cref="CatchAction"/> If no action is provided only console log will be used</param>
    /// <returns></returns>
    public async Task ProcessError(string message, LogLevel logLevel, params CatchAction[] actions)
    {
        await Task.Run(async () =>
        {
            if (actions is null)
            {
                Console.WriteLine(message);
                return;
            }

            bool consoleExecuted = false;

            foreach (var action in actions)
            {
                switch (action)
                {
                    case CatchAction.Console:
                    case CatchAction.Azure:
                    case CatchAction.AWS:
                        if (consoleExecuted is false)
                        {
                            await LogMessageToConsole(logLevel, message);
                            consoleExecuted = true;
                        }
                        break;

                    case CatchAction.Slack:
                        await LogMessageToSlack(logLevel, message);
                        break;

                    case CatchAction.Email:
                        await LogMessageToEmail(logLevel, message);
                        break;

                    default:
                        break;
                }
            }
        });
    }

    private async Task LogMessageToConsole(LogLevel logLevel, string message)
    {
        SetColors(logLevel);
        await LogMessage(logLevel, message);
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
    /// Sends an email when level of the loglevel is met
    /// </summary>
    /// <param name="level">set level on when to log using email</param>
    private async Task LogMessageToEmail(LogLevel logLevel, string message)
    {
        try
        {
            if (RegistratedProcessors.Email is null)
            {
                throw new ArgumentNullException("Email is not injected");
            }
            string subject = $"{logLevel.ToString().ToUpper()} | {RegistratedProcessors.ApplicaionName}, {RegistratedProcessors.Version}";
            string body = $"{logLevel.ToString().ToUpper()} | {DateTime.UtcNow} | {message}";

            foreach (var email in RegistratedProcessors.Email.Copies)
            {
                _fluentEmail.To(email.EmailAddress, email.Name);
            }

            _fluentEmail.Subject(subject);
            _fluentEmail.Body(body);

            await _fluentEmail.SendAsync();
        }
        catch (Exception ex)
        {
            throw;
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
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                break;

            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;

            case LogLevel.Critical:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;

            case LogLevel.None:
                break;

            default:
                break;
        }
    }
}