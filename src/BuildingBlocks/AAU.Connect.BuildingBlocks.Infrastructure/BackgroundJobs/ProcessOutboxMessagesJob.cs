using AAU.Connect.BuildingBlocks.Application.Common.Interfaces;
using AAU.Connect.BuildingBlocks.Domain;
using AAU.Connect.BuildingBlocks.Domain.Outbox;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace AAU.Connect.BuildingBlocks.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob<TDbContext> : IJob
    where TDbContext : DbContext, IOutboxDbContext
{
    private readonly TDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProcessOutboxMessagesJob(TDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        if (messages.Count == 0)
        {
            return;
        }

        foreach (var outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                outboxMessage.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            if (domainEvent is null)
            {
                continue;
            }

            await _publishEndpoint.Publish(domainEvent, context.CancellationToken);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
