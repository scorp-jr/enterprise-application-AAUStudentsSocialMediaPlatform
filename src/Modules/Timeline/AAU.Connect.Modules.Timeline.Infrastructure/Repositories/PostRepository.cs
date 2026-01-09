using AAU.Connect.Modules.Timeline.Infrastructure.Persistence;
using AAU.Connect.Modules.Timeline.Domain.Aggregates;
using AAU.Connect.Modules.Timeline.Domain.ValueObjects;
using AAU.Connect.Modules.Timeline.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Timeline.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly TimelineDbContext _context;

    public PostRepository(TimelineDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Post entity, CancellationToken cancellationToken = default)
    {
        await _context.Posts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Post>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Posts
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Post>> GetLatestPostsAsync(int count = 20)
    {
        return await _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
