// using OpenTelemetry.Trace;
// using OpenTelemetry;
// using System.Diagnostics;
// using Microsoft.ApplicationInsights.Extensibility


// using Moq;
// using Microsoft.Extensions.DependencyInjection;

// namespace Common.Logging.Tests
// {
//     public class AddMyTelemetrySetupTests
//     {
//         [Fact]

//         public void AddTelemetryConfiguration_ShouldConfigureCorrectly()
//         {
//             // Arrange
//             var services = new ServiceCollection();
//             var serviceName = "TestService";

//             // Act
//             services.AddOpenTelemetry(builder => builder
//                 .AddTelemetryConfiguration(serviceName));

//             // Assert
//             // Here you should assert that the correct methods were called on the builder.
//             // This will depend on what AddTelemetryConfiguration does.
//             // For example, if it calls AddSource with the service name, you could check that AddSource was called with the correct argument.
//             builderMock.Verify(b => b.AddSource(serviceName), Times.Once);
//         }

//         //     public async Task AddTelemetryConfigurationTests()
//         //     {
//         //         var testProcessor = new TestActivityProcessor(new DummyExporter<Activity>());
//         //         var source = new ActivitySource("test");

//         //         var tracerProvider = Sdk.CreateTracerProviderBuilder()
//         //             .AddSource("test") // Listen to the ActivitySource named "test"
//         //             .SetSampler(new AlwaysOnSampler())
//         //             .AddProcessor(testProcessor)
//         //             .Build();

//         //         using (source.StartActivity("test span", ActivityKind.Internal))
//         //         {
//         //             // Simulate some work by delaying for a short period
//         //             await Task.Delay(10);
//         //         }

//         //         // Shutdown the TracerProvider to export all remaining activities
//         //         tracerProvider.Shutdown();

//         //         var spans = testProcessor.GetExportedItems();

//         //         // Assert on the spans
//         //         Assert.Single(spans);

//         //     }

//         // }
//     }
// }