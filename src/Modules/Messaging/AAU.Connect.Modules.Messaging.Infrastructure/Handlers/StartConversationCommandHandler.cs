using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Messaging.Application.Commands;
using AAU.Connect.Modules.Messaging.Domain.Aggregates;
using AAU.Connect.Modules.Messaging.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Messaging.Infrastructure.Handlers;

public class StartConversationCommandHandler(MessagingDbContext context) : ICommandHandler<StartConversationCommand, Guid>
{
    public async Task<Guid> Handle(StartConversationCommand command, CancellationToken cancellationToken)
    {
        var existingConversation = await context.Conversations
            .AsNoTracking()
            .ToListAsync(cancellationToken);
            
        var match = existingConversation.FirstOrDefault(c => 
            c.ParticipantIds.Contains(command.InitiatorId) && 
            c.ParticipantIds.Contains(command.RecipientId));

        if (match is not null)
            return match.Id;

        var conversation = Conversation.Start(command.InitiatorId, command.RecipientId);

        context.Conversations.Add(conversation);
        await context.SaveChangesAsync(cancellationToken);

        return conversation.Id;
    }
}

