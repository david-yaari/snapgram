using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace Common.Logging.Serilog;

public static class Extensions
{

    public static IHostBuilder UseSerilogWithElasticsearch(
        this IHostBuilder builder,
        IConfiguration configuration
    )
    {
        var configErrorLogger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var elasticsearchUrl = configuration["ElasticsearchUrl"];
        var seqUrl = configuration["SeqUrl"];

        if (string.IsNullOrWhiteSpace(elasticsearchUrl))
        {
            configErrorLogger.Error("Elasticsearch URL is not configured");
            throw new ArgumentNullException("Elasticsearch URL is not configured");
        }

        if (string.IsNullOrWhiteSpace(seqUrl))
        {
            configErrorLogger.Error("Seq URL is not configured");
            throw new ArgumentNullException("Seq URL is not configured");
        }

        try
        {
            builder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)

                    .Enrich.WithMachineName() // Add machine name enricher
                    .Enrich.WithProcessId() // Add process ID enricher
                    .Enrich.WithThreadId() // Add thread ID enricher
                    .Enrich.FromLogContext() // Add ASP.NET Core HTTP context enricher
                    .Enrich.WithExceptionDetails() // Add exception details enricher
                    .Enrich.WithEnvironmentName() // Add environment name enricher

                    .WriteTo.Console(
                        theme: AnsiConsoleTheme.Code,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    ) // Configure console sink with AnsiConsoleTheme.Code theme and specified output template

                    .WriteTo.Debug(
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    ) // Add Debug sink

                    .WriteTo.Seq(seqUrl) // Add Seq sink

                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                    {
                        IndexFormat = $"{hostingContext.Configuration["AppName"]}-logs-{hostingContext.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1
                    });
            });
        }
        catch (Exception ex)
        {
            configErrorLogger.Error(ex, "Error configuring Serilog with Elasticsearch and Seq");
            throw;
        }
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        return builder;
    }
}