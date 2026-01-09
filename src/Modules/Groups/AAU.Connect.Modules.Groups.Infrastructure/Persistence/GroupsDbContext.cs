using AAU.Connect.Modules.Groups.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using AAU.Connect.BuildingBlocks.Domain.Outbox;
using AAU.Connect.BuildingBlocks.Application.Common.Interfaces;

namespace AAU.Connect.Modules.Groups.Infrastructure.Persistence;

public class GroupsDbContext : DbContext, IOutboxDbContext
{
    public GroupsDbContext(DbContextOptions<GroupsDbContext> options) : base(options)
    {
    }

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("groups");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GroupsDbContext).Assembly);
    }
}
