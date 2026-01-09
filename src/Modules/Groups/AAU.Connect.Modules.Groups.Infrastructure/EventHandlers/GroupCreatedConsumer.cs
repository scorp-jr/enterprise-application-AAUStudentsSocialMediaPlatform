using AAU.Connect.Modules.Groups.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AAU.Connect.Modules.Groups.Infrastructure.EventHandlers;

public class GroupCreatedConsumer : IConsumer<GroupCreatedDomainEvent>
{
    private readonly ILogger<GroupCreatedConsumer> _logger;

    public GroupCreatedConsumer(ILogger<GroupCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<GroupCreatedDomainEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation(
            "Group created event received: GroupId={GroupId}, Name={Name}, CreatorId={CreatorId}",
            @event.GroupId,
            @event.Name,
            @event.CreatorId);

        await Task.CompletedTask;
    }
}
