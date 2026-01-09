using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Messaging.Application.Queries;
using AAU.Connect.Modules.Messaging.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Messaging.Infrastructure.Handlers;

public class GetMessagesQueryHandler(MessagingDbContext context) : IQueryHandler<GetMessagesQuery, List<MessageDto>>
{
    public async Task<List<MessageDto>> Handle(GetMessagesQuery query, CancellationToken cancellationToken)
    {
        return await context.Messages
            .Where(m => m.ConversationId == query.ConversationId)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new MessageDto(m.Id, m.SenderId, m.Content, m.CreatedAt ?? DateTime.UtcNow))
            .ToListAsync(cancellationToken);
    }
}

