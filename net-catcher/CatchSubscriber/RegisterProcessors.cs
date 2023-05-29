using CatchSubscriber.Models;
using Slack.Webhooks;

namespace CatchSubscriber;

public static class RegisterProcessors
{
    private static SlackProcessor? SlackProcessor = null;

    public static void InjectAWSXRay()
    {
    }

    public static void InjectAzureDiagnostics()
    {
    }

    public static SlackProcessor InjectSlack(string hookUrl, string channel, string userName, string emoji = "")
    {
        SlackClient slackClient = new(hookUrl);

        var slackMessage = new SlackMessage
        {
            Channel = $"#{channel}",
            IconEmoji = string.IsNullOrEmpty(emoji) is true ? Emoji.AlarmClock : emoji,
            Username = userName
        };
        SlackProcessor slackProcessor = new(slackClient, slackMessage);

        return slackProcessor;
    }
}