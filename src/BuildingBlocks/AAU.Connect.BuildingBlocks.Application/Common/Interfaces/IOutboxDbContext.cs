using AAU.Connect.BuildingBlocks.Domain.Outbox;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.BuildingBlocks.Application.Common.Interfaces;

public interface IOutboxDbContext
{
    DbSet<OutboxMessage> OutboxMessages { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
