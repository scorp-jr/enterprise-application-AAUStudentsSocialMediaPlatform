using AAU.Connect.Modules.Auth.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AAU.Connect.Modules.Auth.Infrastructure.EventHandlers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredDomainEvent>
{
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegisteredDomainEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation(
            "User registered event received: UserId={UserId}, Email={Email}",
            @event.UserId,
            @event.Email);

        await Task.CompletedTask;
    }
}
