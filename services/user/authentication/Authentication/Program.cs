using Serilog;
using Common.Logging.Serilog;
using Common.Logging.OpenTelemetry;
//using Common.Logging.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.HttpLogging;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using Common.Authentication.JwtAuthentication;
using Authentication.Service;
using Common.DbEventStore.MongoDB;
using Common.DbEventStore.Settings.Service;
using Common.DbEventStore.MassTransit;
using Authentication.Entities;
using Authentication.Extensions;
using Authentication;
using Authentication.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.
services.AddCors();

// Add services to the container.
services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Add Logging
builder.Host.UseSerilogWithElasticsearch(configuration);

var serviceName = "Authentication";

// Configure telemetry
services.AddOpenTelemetry()
    .WithTracing(builder => builder
    .AddTelemetryConfiguration(serviceName));


services
    .AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddSingleton<JwtTokenValidator>();
builder.Services.AddSingleton<UserAuthenticationService>();

services.AddHttpLogging(logging =>
{
    // Configure logging options here...
    logging.LoggingFields = HttpLoggingFields.All;
});

var collectionName = configuration[$"{nameof(ServiceSettings)}:{nameof(ServiceSettings.ItemsCollectionName)}"]!;

services.AddMongo()
.AddMongoRepository<Tenant>(collectionName)
.AddMongoRepository<User>(collectionName)
.AddMongoRepository<Role>(collectionName)
.AddMassTransitWithRabbitMq();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.Logger.LogInformation(5, "Application started using...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.WithOrigins("http://localhost:3000")
          .AllowAnyMethod()
          .AllowAnyHeader());

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
