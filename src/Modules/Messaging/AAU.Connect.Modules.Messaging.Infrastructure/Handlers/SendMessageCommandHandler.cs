using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Messaging.Application.Commands;
using AAU.Connect.Modules.Messaging.Domain.Entities;
using AAU.Connect.Modules.Messaging.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Messaging.Infrastructure.Handlers;

public class SendMessageCommandHandler(MessagingDbContext context) : ICommandHandler<SendMessageCommand, Guid>
{
    public async Task<Guid> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var conversation = await context.Conversations
            .FirstOrDefaultAsync(c => c.Id == command.ConversationId, cancellationToken);

        if (conversation is null)
            throw new ArgumentException("Conversation not found");

        var message = new Message(command.ConversationId, command.SenderId, command.Content);
        
        conversation.UpdateLastMessage(command.Content);

        context.Messages.Add(message);
        await context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}
