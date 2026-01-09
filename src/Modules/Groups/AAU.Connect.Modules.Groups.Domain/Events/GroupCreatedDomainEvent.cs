using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Groups.Domain.Events;

public record GroupCreatedDomainEvent(Guid GroupId, string Name, Guid CreatorId) : IDomainEvent;
