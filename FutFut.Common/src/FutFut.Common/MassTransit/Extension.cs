using System.Reflection;
using FutFut.Common.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FutFut.Common.MassTransit;

public static class Extension
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services,
        Action<IRetryConfigurator>? configureRetries = null)
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumers(Assembly.GetEntryAssembly());

            configure.UsingRabbitMq((context, configurator) =>
            {
                var configuration = context.GetRequiredService<IConfiguration>();
                var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>()!;
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;

                configurator.Host(rabbitMQSettings.Host);

                configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

                if (configureRetries == null)
                {
                    configureRetries = retryConfig =>
                    {
                        retryConfig.Interval(3, TimeSpan.FromSeconds(5));
                    };
                }

                configurator.UseMessageRetry(configureRetries);
            });
        });

        return services;
    }
}