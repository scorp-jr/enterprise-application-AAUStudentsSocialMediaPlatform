using AAU.Connect.Modules.Messaging.Domain.Aggregates;
using AAU.Connect.Modules.Messaging.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AAU.Connect.BuildingBlocks.Domain.Outbox;
using AAU.Connect.BuildingBlocks.Application.Common.Interfaces;

namespace AAU.Connect.Modules.Messaging.Infrastructure.Persistence;

public class MessagingDbContext : DbContext, IOutboxDbContext
{
    public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options)
    {
    }

    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("messaging");
        
        modelBuilder.Entity<Conversation>()
            .Property(c => c.ParticipantIds)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());
                
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessagingDbContext).Assembly);
    }
}
