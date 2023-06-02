using CatchSubscriber.Models;
using FluentAssertions;

namespace Example.IntegrationTests;

public class EmailProcessor : IntegrationTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ShouldProcessErrorUsingEmail()
    {
        //TODO FIX THIS 
        //Given
        GetRequiredService<IErrorProcesser>(out var service);

        //await service.ProcessError("This is my message", LogLevel.Warning, CatchSubscriber.Models.CatchAction.Email);

        //When
        //Func<Task> outcome = async () => await service.ProcessError("This is my message", LogLevel.Warning, CatchSubscriber.Models.CatchAction.Email);
        //Action act = () => outcome();

        //Then
        //act.Should().NotThrow();

        Assert.Fail();
    }

    [Test]
    public async Task ShouldProcessUsingDefaultAction()
    {
        GetRequiredService<IErrorProcesser>(out var service);
        LogLevel expectedLogLevel = LogLevel.Debug;

        await service.ProcessError("This usnig defalut action to log a message", expectedLogLevel);
    }

    [Test]
    public async Task ShouldProcessMessageUsingMultiplyActions()
    {
        GetRequiredService<IErrorProcesser>(out var service);

        await service.ProcessError("This is my log message", LogLevel.Critical, CatchAction.Azure, CatchAction.Console);
    }

    [Test]
    public void ShouldProcessMessageUsingAzureDiagnostics()
    {
        //Given
        GetRequiredService<IErrorProcesser>(out var service);

        LogLevel expectedLogLevel = LogLevel.Critical;

        //When
        Func<Task> outcome = async () => await service.ProcessError("This is my message for azure diagnostics", expectedLogLevel, CatchAction.Azure);
        Action act = () => outcome();

        //Then
        act.Should().NotThrow();
    }

    [Test]
    public void ShouldProcessMessageUsingAWSXRay()
    {
        //Given
        GetRequiredService<IErrorProcesser>(out var service);

        LogLevel expectedLogLevel = LogLevel.Critical;

        //When
        Func<Task> outcome = async () => await service.ProcessError("This is my message for AWSXRay", expectedLogLevel, CatchAction.AWS);
        Action act = () => outcome();

        //Then
        act.Should().NotThrow();
    }

    [Test]
    public void ShouldProcessMessageUsingSlackWebbHook()
    {
        //Given
        GetRequiredService<IErrorProcesser>(out var service);

        service.RegisterSlack("MyTestApplication", "1.0", "https://hooks.slack.com/services/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", "Testing", "ThisApplication");

        var expectedActions = CatchAction.Slack;

        //When
        Func<Task> outcome = async () => await service.ProcessError("This is a test message", LogLevel.Critical, expectedActions);
        Action act = () => outcome();

        //Then
        act.Should().NotThrow();
    }
}