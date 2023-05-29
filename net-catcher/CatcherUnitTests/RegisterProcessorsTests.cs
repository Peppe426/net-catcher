using CatchSubscriber;
using FluentAssertions;
using Slack.Webhooks;

namespace CatcherUnitTests;

public class RegisterProcessorsTests
{
    [Test]
    public void ShouldThrowArgumentExceptionWhenRegisterSlack()
    {
        //Given
        ErrorProcessor ProcessError = new();

        //When
        Action outcome = () => ProcessError.RegisterSlack("myWebhookUrl", "mychannel", "ApplicationName", Emoji.AlarmClock.ToString());

        //Then
        outcome.Should().Throw<ArgumentException>();
    }
}