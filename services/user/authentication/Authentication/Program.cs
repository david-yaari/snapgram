using Serilog;
using Common.Logging.Serilog;
using Common.Logging.OpenTelemetry;
//using Common.Logging.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using Common.Authentication.JwtAuthentication;
using Common.Authentication.Repositories;
using Authentication.Service;
using Authentication.Service.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Logging
builder.Host.UseSerilogWithElasticsearch(builder.Configuration);

var serviceName = "Authentication";

// Configure telemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder
    .AddTelemetryConfiguration(serviceName));


builder.Services
    .AddSingleton<UserRepository>()
    .AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSingleton<JwtTokenHandler, JwtTokenHandler>();

builder.Services.AddHttpLogging(logging =>
{
    // Configure logging options here...
    logging.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

app.Logger.LogInformation(5, "Application started using...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpLogging();

app.UseAuthorization();

app.UseStatusCodePages();
app.UseExceptionHandler();

app.MapAuthenticateEndpoints();


// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
