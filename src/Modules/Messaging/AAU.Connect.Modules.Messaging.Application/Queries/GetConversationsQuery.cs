using AAU.Connect.BuildingBlocks.Application;

namespace AAU.Connect.Modules.Messaging.Application.Queries;

public record GetConversationsQuery(Guid UserId) : IQuery<List<ConversationDto>>;
public record ConversationDto(Guid Id, List<Guid> ParticipantIds, string LastMessagePreview, DateTime LastMessageAt);

