using CatchSubscriber.Processors;
using Slack.Webhooks;

namespace CatchSubscriber;

public class ProcessorHandler
{
    internal SlackProcessor? Slack = null;

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
}