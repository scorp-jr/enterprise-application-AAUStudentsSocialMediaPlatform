using System;
using System.Threading;
using System.Threading.Tasks;

namespace AAU.Connect.Modules.Auth.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Guid> CreateUserAsync(string email, string password, string firstName, string lastName, CancellationToken cancellationToken = default);
    }
}
