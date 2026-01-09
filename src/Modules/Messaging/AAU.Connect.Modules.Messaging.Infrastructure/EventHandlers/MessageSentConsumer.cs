using AAU.Connect.Modules.Messaging.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AAU.Connect.Modules.Messaging.Infrastructure.EventHandlers;

public class MessageSentConsumer : IConsumer<MessageSentDomainEvent>
{
    private readonly ILogger<MessageSentConsumer> _logger;

    public MessageSentConsumer(ILogger<MessageSentConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MessageSentDomainEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation("Message sent event received: MessageId={MessageId}", @event.MessageId);
        await Task.CompletedTask;
    }
}
