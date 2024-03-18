using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.DbEventStore.Settings;
using Common.DbEventStore.Settings.Service;
using Common.DbEventStore.Settings.RabbitMQ;

namespace Common.DbEventStore.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetRequiredService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    if (serviceSettings is null || rabbitMQSettings is null)
                    {
                        throw new ArgumentNullException("ServiceSettings or RabbitMQSettings");
                    }
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                    configurator.UseMessageRetry(retryConfigigurator =>
                    {
                        retryConfigigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });
            //services.AddMassTransitHostedService(); Deprecated
            return services;
        }
    }
};