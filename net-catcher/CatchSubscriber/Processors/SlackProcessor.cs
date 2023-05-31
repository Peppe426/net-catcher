using Microsoft.Extensions.Logging;
using Slack.Webhooks;

namespace CatchSubscriber.Processors;

public class SlackProcessor
{
    public SlackClient SlackClient { get; private set; }
    public SlackMessage SlackMessage { get; private set; }

    public SlackProcessor(SlackClient slackClient, SlackMessage slackMessage)
    {
        SlackClient = slackClient;
        SlackMessage = slackMessage;
    }

    public SlackProcessor SetMessage(LogLevel logLevel, string message)
    {
        //TODO add emoji/textdecorations for diffrent loglevels
        SlackMessage.Text = message;
        return this;
    }

    public SlackProcessor SetChannel(string channel)
    {
        SlackMessage.Channel = channel;
        return this;
    }
}