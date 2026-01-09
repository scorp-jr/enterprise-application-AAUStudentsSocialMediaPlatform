using AAU.Connect.Modules.Timeline.Domain.Entities;
using AAU.Connect.Modules.Timeline.Domain.Aggregates;
using AAU.Connect.Modules.Timeline.Domain.Interfaces;
using AAU.Connect.Modules.Timeline.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using AAU.Connect.BuildingBlocks.Domain.Outbox;
using AAU.Connect.BuildingBlocks.Application.Common.Interfaces;

namespace AAU.Connect.Modules.Timeline.Infrastructure.Persistence;

public class TimelineDbContext : DbContext, IOutboxDbContext
{
    public TimelineDbContext(DbContextOptions<TimelineDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<PostLike> PostLikes => Set<PostLike>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("timeline");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimelineDbContext).Assembly);
    }
}
