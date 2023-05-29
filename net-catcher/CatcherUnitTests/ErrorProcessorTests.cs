using CatchSubscriber;
using CatchSubscriber.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace CatcherUnitTests;

public class ErrorProcessorTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ShouldProcessUsingDefaultAction()
    {
        ErrorProcessor ProcessError = new();
        LogLevel expectedLogLevel = LogLevel.Debug;

        await ProcessError.ProcessError("This usnig defalut action to log a message", expectedLogLevel);
    }

    [Test]
    public async Task ShouldProcessMessageUsingMultiplyActions()
    {
        ErrorProcessor ProcessError = new();
        var expectedActions = new List<CatchAction>()
        {
            CatchAction.Azure,
            CatchAction.Console
        };

        await ProcessError.ProcessError("This is my log message", LogLevel.Critical, expectedActions);
    }

    [Test]
    public async Task ShouldProcessMessageUsingAzureDiagnostics()
    {
        //Given
        ErrorProcessor ProcessError = new();
        LogLevel expectedLogLevel = LogLevel.Critical;
        var expectedActions = new List<CatchAction>()
        {
            CatchAction.Azure
        };

        //When
        Func<Task> outcome = async () => await ProcessError.ProcessError("This is my message for azure diagnostics", expectedLogLevel, expectedActions);
        Action act = () => outcome();

        //Then
        act.Should().NotThrow();
    }

    [Test]
    public async Task ShouldProcessMessageUsingAWSXRay()
    {
        //Given
        ErrorProcessor ProcessError = new();
        LogLevel expectedLogLevel = LogLevel.Critical;
        var expectedActions = new List<CatchAction>()
        {
            CatchAction.AWS
        };

        //When
        Func<Task> outcome = async () => await ProcessError.ProcessError("This is my message for AWSXRay", expectedLogLevel, expectedActions);
        Action act = () => outcome();

        //Then
        act.Should().NotThrow();
    }

    [Test]
    public async Task ShouldProcessMessageUsingSlackWebbHook()
    {
        //Given
        ErrorProcessor ProcessError = new();
        ProcessError.RegisterSlack("https://hooks.slack.com/services/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", "Testing", "ThisApplication");

        var expectedActions = new List<CatchAction>()
        {
            CatchAction.Slack
        };

        //When
        Func<Task> outcome = async () => await ProcessError.ProcessError("This is a test message", LogLevel.Critical, expectedActions);
        Action act = () => outcome();

        //Then
        act.Should().NotThrow();
    }

    [Test]
    public async Task ShouldProcessMessageUsingEmail()
    {
        //Given
        ErrorProcessor ProcessError = new();
        LogLevel expectedLogLevel = LogLevel.Critical;
        var expectedActions = new List<CatchAction>()
        {
            CatchAction.Email
        };

        //When
        await ProcessError.ProcessError("This is my message for an Email", expectedLogLevel, expectedActions);

        //Then

        Assert.Fail();
    }
}