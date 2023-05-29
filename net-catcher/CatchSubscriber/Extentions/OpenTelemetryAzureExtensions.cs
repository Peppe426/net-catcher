using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static partial class OpenTelemetryExtensions
{
    public static void AddOpenTelemetryForAzure(this IServiceCollection services, string serviceName, string serviceVersion, string azureMonitorConnectionString)
    {
        services.AddOpenTelemetry()
           .WithTracing(builder =>
           {
               builder
               .AddSource(serviceName)
               .AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddAzureMonitorTraceExporter(options =>
                {
                    options.ConnectionString = azureMonitorConnectionString;
                })
               .ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: serviceVersion));
           });
    }
}