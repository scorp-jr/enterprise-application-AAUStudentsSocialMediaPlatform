using AAU.Connect.Modules.Auth.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AAU.Connect.Modules.Auth.Infrastructure.Identity
{
    public class KeycloakService : IIdentityService
    {
        private readonly IConfiguration _configuration;

        public KeycloakService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Guid> CreateUserAsync(string email, string password, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            
            return Guid.NewGuid();
        }

        public string GetCurrentUserId()
        {
            return "system";
        }
    }
}
