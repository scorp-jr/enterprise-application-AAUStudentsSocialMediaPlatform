using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Timeline.Domain.Events;

public record PostCreatedDomainEvent(Guid PostId, Guid UserId) : IDomainEvent;
