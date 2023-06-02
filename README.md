# net-catcher

The "net-catcher" repository is designed to simplify the configuration and utilization of OpenTelemetry for collecting and exporting telemetry data, including metrics, logs, and distributed traces. Its purpose is to streamline the monitoring and troubleshooting process for complex distributed systems by providing a unified approach to telemetry collection.

By utilizing OpenTelemetry in AWS and Azure with the help of "net-catcher," you can enhance the visibility of your cloud-native applications and infrastructure. This heightened visibility allows you to proactively identify and address performance issues, optimize resource utilization, and improve the overall reliability and availability of your systems.

Additionally, "net-catcher" extends its support to log integration with platforms such as Slack, email, and SMS. This feature enables you to conveniently route logs to these communication channels, facilitating efficient and timely notifications and alerts.

## How to
Clone this repo or download the nuget.
Using DI in Program.cs do one or more of the following:

### Add OpenTelemetry for AWS
This example demonstrates how to add OpenTelemetry for AWS in your application. It configures OpenTelemetry to collect telemetry data specific to AWS services and resources, enabling you to monitor and troubleshoot your AWS-based applications effectively.
```
builder.Services.AddOpenTelemetryForAWS("ThisApplication", "1.0");
```


### Add OpenTelemetry for Azure
By incorporating OpenTelemetry for Azure, you can monitor and analyze metrics, logs, and distributed traces from your Azure-based applications.
Replace `[AzureConnectionString]` with your actual Azure connection string. This step allows OpenTelemetry to establish a connection and collect telemetry data from Azure resources.
```
builder.Services.AddOpenTelemetryForAzure("ThisApplication", "1.0", "[AzureConnectionString]"); 
```

### Add Slack Integration
This example illustrates how to integrate Slack with OpenTelemetry for sending error notifications and alerts. By registering the Slack webhook URL, you can receive error notifications directly in your Slack workspace, enabling you to stay informed about any issues occurring in your application.
You must first enable the Webhooks integration for your Slack Account to get the Token. You can enable it here: https://slack.com/services/new/incoming-webhook
```
builder.Services.AddScoped<IErrorProcesser, ErrorProcessor>(
    serviceProvider => new ErrorProcessor()
    .RegisterSlack("https://hooks.slack.com/services/[YOUR-TOKEN]", "Testing", userName: "ThisApplication")
);
```

### Add Email integration