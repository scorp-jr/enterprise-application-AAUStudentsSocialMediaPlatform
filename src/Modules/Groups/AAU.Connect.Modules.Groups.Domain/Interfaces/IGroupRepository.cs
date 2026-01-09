using AAU.Connect.Modules.Groups.Domain.Aggregates;
using AAU.Connect.Modules.Groups.Domain.ValueObjects;
using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Groups.Domain.Interfaces;

public interface IGroupRepository : IRepository<Group>
{
    Task<Group?> GetByIdAsync(Guid id);
    Task<List<Group>> GetAllAsync(GroupType? type = null);
    Task<List<Group>> GetByUserIdAsync(Guid userId);
}
