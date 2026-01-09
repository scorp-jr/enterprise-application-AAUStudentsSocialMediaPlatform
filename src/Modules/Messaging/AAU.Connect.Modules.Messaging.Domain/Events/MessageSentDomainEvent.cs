using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Messaging.Domain.Events;

public record MessageSentDomainEvent(Guid MessageId, Guid ConversationId, Guid SenderId) : IDomainEvent;
