using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace Common.Logging.OpenTelemetry;

public static class Extensions
{
    public static TracerProviderBuilder AddTelemetryConfiguration(this TracerProviderBuilder builder, string serviceName)
    {
        return builder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddSource(serviceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddJaegerExporter(jaegerOptions =>
            {
                jaegerOptions.AgentHost = "localhost";
                jaegerOptions.AgentPort = 6831;
            });
    }
}
