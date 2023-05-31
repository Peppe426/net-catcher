using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests.Base;

public class IntegrationTest : IDisposable
{
    private TestWebApplicationFactory _appFactory = default!;
    public IServiceProvider ServiceProvider = default!;

    protected HttpClient HttpClient { get; private set; } = default!;

    public IntegrationTest()
    {
        _appFactory = new TestWebApplicationFactory();
        HttpClient = _appFactory.CreateClient();
        ServiceProvider = _appFactory.Services;
    }

    protected AsyncServiceScope GetRequiredService<T>(out T service) where T : notnull
    {
        var scope = ServiceProvider.CreateAsyncScope();

        try
        {
            service = scope.ServiceProvider.GetRequiredService<T>();
        }
        catch
        {
            scope.Dispose();
            throw;
        }

        return scope;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _appFactory.Dispose();
        }
    }
}