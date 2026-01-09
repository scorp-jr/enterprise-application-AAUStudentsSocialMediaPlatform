using AAU.Connect.Modules.Timeline.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AAU.Connect.Modules.Timeline.Infrastructure.EventHandlers;

public class PostCreatedConsumer : IConsumer<PostCreatedDomainEvent>
{
    private readonly ILogger<PostCreatedConsumer> _logger;

    public PostCreatedConsumer(ILogger<PostCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PostCreatedDomainEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation("Post created event received: PostId={PostId}, UserId={UserId}", @event.PostId, @event.UserId);
        await Task.CompletedTask;
    }
}
