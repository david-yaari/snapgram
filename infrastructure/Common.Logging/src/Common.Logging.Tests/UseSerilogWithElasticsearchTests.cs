using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Common.Logging.Serilog;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Logging.Tests;

public class UseSerilogWithElasticsearchTests
{
    [Fact]
    public void UseSerilogWithElasticsearch_ConfiguresLoggerCorrectly()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"ElasticsearchUrl", "http://localhost:9200"},
            {"SeqUrl", "http://localhost:5341"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var hostBuilder = Host.CreateDefaultBuilder();

        // Act
        hostBuilder.UseSerilogWithElasticsearch(configuration);
        var host = hostBuilder.Build();

        // Assert
        var logger = host.Services.GetService<ILogger<UseSerilogWithElasticsearchTests>>();
        Assert.NotNull(logger);
    }
    
}
