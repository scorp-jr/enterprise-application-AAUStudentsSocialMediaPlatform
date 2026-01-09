using AAU.Connect.Modules.Auth.Application.Common.Interfaces;
using AAU.Connect.Modules.Auth.Domain.Interfaces;
using AAU.Connect.Modules.Auth.Infrastructure.Identity;
using AAU.Connect.Modules.Auth.Infrastructure.Persistence;
using AAU.Connect.Modules.Auth.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AAU.Connect.BuildingBlocks.Domain.Outbox;
using AAU.Connect.BuildingBlocks.Infrastructure.Outbox;

namespace AAU.Connect.Modules.Auth.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AuthDb");
            services.AddScoped<InsertOutboxMessagesInterceptor>();
            
            services.AddDbContext<AuthDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptor>();
                options.UseNpgsql(connectionString)
                       .AddInterceptors(interceptor);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IIdentityService, KeycloakService>();

            return services;
        }
    }
}
