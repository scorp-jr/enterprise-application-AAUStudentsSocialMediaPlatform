using AAU.Connect.BuildingBlocks.Domain;
using AAU.Connect.Modules.Timeline.Domain.Aggregates;

namespace AAU.Connect.Modules.Timeline.Domain.Interfaces;

public interface IPostRepository : IRepository<Post>
{
    Task<Post?> GetByIdAsync(Guid id);
    Task<List<Post>> GetByUserIdAsync(Guid userId);
    Task<List<Post>> GetLatestPostsAsync(int count = 20);
}
