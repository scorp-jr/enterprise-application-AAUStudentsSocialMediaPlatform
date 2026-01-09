using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AAU.Connect.BuildingBlocks.Infrastructure.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        params Type[] consumerAssemblyMarkerTypes)
    {
        services.AddMassTransit(x =>
        {
            foreach (var markerType in consumerAssemblyMarkerTypes)
            {
                x.AddConsumers(markerType.Assembly);
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqHost = configuration["RabbitMQ:Host"] ?? "localhost";
                var rabbitMqUser = configuration["RabbitMQ:Username"] ?? "guest";
                var rabbitMqPass = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(rabbitMqHost, "/", h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPass);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
