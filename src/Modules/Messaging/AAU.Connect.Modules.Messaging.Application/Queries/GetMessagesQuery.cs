using AAU.Connect.BuildingBlocks.Application;

namespace AAU.Connect.Modules.Messaging.Application.Queries;

public record GetMessagesQuery(Guid ConversationId) : IQuery<List<MessageDto>>;
public record MessageDto(Guid Id, Guid SenderId, string Content, DateTime CreatedAt);

