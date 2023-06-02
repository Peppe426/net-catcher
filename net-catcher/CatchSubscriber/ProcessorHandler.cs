using CatchSubscriber.Processors;
using Slack.Webhooks;

namespace CatchSubscriber;

public class ProcessorHandler
{
    internal SlackProcessor? Slack = null;
    internal EmailProcessor? Email = null;

    public string ApplicaionName { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;

    public ProcessorHandler InjectSlack(string hookUrl, string channel, string userName, string emoji = "")
    {
        SlackClient slackClient = new(hookUrl);

        var slackMessage = new SlackMessage
        {
            Channel = $"#{channel}",
            IconEmoji = string.IsNullOrEmpty(emoji) is true ? Emoji.AlarmClock : emoji,
            Username = userName
        };

        Slack = new(slackClient, slackMessage);

        return this;
    }

    //internal ProcessorHandler InjectEmail(string fromEmail, string toEmail, string fromName = "", string toName = "")
    //{
    //    fromEmail.IsEmail();
    //    toEmail.IsEmail();
    //    _fluentEmail.SetFrom(fromEmail, fromName);
    //    _fluentEmail.To(toEmail, toName);
    //    return this;
    //}
    internal ProcessorHandler InjectEmail(string toEmail, string toName = "", List<(string emailAddress, string? name)>? copies = null)
    {
        Email = new();
        Email.AddCopy((toEmail, toName));
        if (copies != null && copies.Any() is true)
        {
            copies.ForEach((email) =>
            {
                Email.AddCopy((email.emailAddress, email.name));
            });
        }

        return this;
    }

    internal ProcessorHandler SetApplicationName(string name)
    {
        ApplicaionName = name;
        return this;
    }

    internal ProcessorHandler SetVersion(string version)
    {
        Version = version;
        return this;
    }
}