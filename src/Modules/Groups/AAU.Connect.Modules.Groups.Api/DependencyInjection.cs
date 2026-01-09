using AAU.Connect.Modules.Groups.Domain.Interfaces;
using AAU.Connect.Modules.Groups.Infrastructure.Persistence;
using AAU.Connect.Modules.Groups.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AAU.Connect.BuildingBlocks.Domain.Outbox;
using AAU.Connect.BuildingBlocks.Infrastructure.Outbox;

namespace AAU.Connect.Modules.Groups.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddGroupsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AuthDb");

        services.AddScoped<InsertOutboxMessagesInterceptor>();

        services.AddDbContext<GroupsDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptor>();
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable("__GroupsMigrationsHistory", "groups"))
                   .AddInterceptors(interceptor);
        });

        services.AddScoped<IGroupRepository, GroupRepository>();

        return services;
    }
}
