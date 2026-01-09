using AAU.Connect.Modules.Groups.Domain.Aggregates;
using AAU.Connect.Modules.Groups.Domain.Interfaces;
using AAU.Connect.Modules.Groups.Domain.ValueObjects;
using AAU.Connect.Modules.Groups.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Groups.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly GroupsDbContext _context;

    public GroupRepository(GroupsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Group entity, CancellationToken cancellationToken = default)
    {
        await _context.Groups.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Group?> GetByIdAsync(Guid id)
    {
        return await _context.Groups
            .Include(g => g.Assignments)
            .Include(g => g.Resources)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<Group>> GetAllAsync(GroupType? type = null)
    {
        var query = _context.Groups.AsQueryable();
        if (type.HasValue)
            query = query.Where(g => g.Type == type.Value);
        
        return await query.ToListAsync();
    }

    public async Task<List<Group>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Groups
            .Where(g => g.MemberIds.Contains(userId))
            .ToListAsync();
    }
}
