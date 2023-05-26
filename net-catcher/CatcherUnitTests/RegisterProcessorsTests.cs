using CatchSubscriber;
using FluentAssertions;
using Slack.Webhooks;

namespace CatcherUnitTests;

public class RegisterProcessorsTests
{
    [Test]
    public async Task ShouldThrowArgumentExceptionWhenRegisterSlack()
    {
        //Given
        ErrorProcessor ProcessError = new();

        //When
        Action action = () => ProcessError.RegisterSlack("myWebhookUrl", "mychannel", "ApplicationName", Emoji.AlarmClock.ToString());

        //Then
        action.Should().Throw<ArgumentException>();
    }
}