using CatchSubscriber;
using CatchSubscriber.Extentions;
using CatchSubscriber.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//------------ EXAMPLE ADD AWS ------------
builder.Services.AddOpenTelemetryForAWS("ThisApplication", "1.0");

//------------ EXAMPLE ADD Azure ------------
builder.Services.AddOpenTelemetryForAzure("ThisApplication", "1.0", "Azure connecitonstring");

//------------ EXAMPLE ADD SLACK ------------
//You must first enable the Webhooks integration for your Slack Account to get the Token. You can enable it here: https://slack.com/services/new/incoming-webhook
builder.Services.AddScoped<IErrorProcesser, ErrorProcessor>(
    serviceProvider => new ErrorProcessor()
    .RegisterSlack("https://hooks.slack.com/services/[YOUR-TOKEN]", "Testing", userName: "ThisApplication")
 );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}