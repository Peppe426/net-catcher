using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CatchSubscriber.Extentions;

public static partial class OpenTelemetryExtensions
{
    /// <summary>
    /// <see href="https://opentelemetry.io/docs/instrumentation/net/manual/#aspnet-core">Instrumentation</see>
    /// <see href="https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/src/OpenTelemetry.Contrib.Instrumentation.AWS/README.md">Instrument AWS</see>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceName"></param>
    /// <param name="serviceVersion"></param>
    public static void AddOpenTelemetryForAWS(this IServiceCollection services, string serviceName, string serviceVersion)
    {
        services.AddOpenTelemetry()
        .WithTracing(builder =>
        {
            builder
            .AddSource(serviceName)
            .AddConsoleExporter()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAWSInstrumentation()
            .ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: serviceVersion));
        });
    }
}