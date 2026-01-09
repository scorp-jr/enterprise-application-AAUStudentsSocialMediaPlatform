using AAU.Connect.BuildingBlocks.Domain;
using AAU.Connect.Modules.Auth.Domain.Aggregates;

namespace AAU.Connect.Modules.Auth.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
}
