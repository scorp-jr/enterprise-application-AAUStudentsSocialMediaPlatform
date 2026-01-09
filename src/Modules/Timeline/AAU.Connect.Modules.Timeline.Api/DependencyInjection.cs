using AAU.Connect.Modules.Timeline.Domain.Aggregates;
using AAU.Connect.Modules.Timeline.Domain.Interfaces;
using AAU.Connect.Modules.Timeline.Domain.ValueObjects;
using AAU.Connect.Modules.Timeline.Infrastructure.Persistence;
using AAU.Connect.Modules.Timeline.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AAU.Connect.BuildingBlocks.Domain.Outbox;
using AAU.Connect.BuildingBlocks.Infrastructure.Outbox;

namespace AAU.Connect.Modules.Timeline.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddTimelineModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AuthDb");

        services.AddScoped<InsertOutboxMessagesInterceptor>();

        services.AddDbContext<TimelineDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptor>();
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable("__TimelineMigrationsHistory", "timeline"))
                   .AddInterceptors(interceptor);
        });

        services.AddScoped<IPostRepository, PostRepository>();

        return services;
    }
}
