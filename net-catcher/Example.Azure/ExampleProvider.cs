using CatchSubscriber.Interfaces;
using CatchSubscriber.Models;
using Example.Api.Interfaces;

namespace Example.Api;

public class ExampleProvider : IExampleProvider
{
    private IErrorProcesser _errorProcesser;

    public ExampleProvider(IErrorProcesser errorProcesser)
    {
        _errorProcesser = errorProcesser;
    }

    public async Task DoSomething(string processThisString)
    {
        if (string.IsNullOrEmpty(processThisString) is false)
        {
            await _errorProcesser.ProcessError("processThisString has trace", LogLevel.Trace, CatchAction.Console);
            await _errorProcesser.ProcessError("processThisString has information", LogLevel.Information, CatchAction.Console);
            await _errorProcesser.ProcessError("processThisString has debug", LogLevel.Debug, CatchAction.Console);
            await _errorProcesser.ProcessError("processThisString has warning", LogLevel.Warning, CatchAction.Console);
            await _errorProcesser.ProcessError("processThisString has Error", LogLevel.Error, CatchAction.Console);
            throw new ArgumentNullException("parameter should not have a value");
        }
    }
}