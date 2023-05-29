using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CatchSubscriber.Extentions;

public static partial class OpenTelemetryExtensions
{
    public static void AddOpenTelemetryForConsole(this IServiceCollection services, string serviceName, string serviceVersion)
    {
        services.AddOpenTelemetry()
           .WithTracing(builder =>
           {
               builder
               .AddSource(serviceName)
               .AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddConsoleExporter()
               .ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: serviceVersion));
           });
    }
}