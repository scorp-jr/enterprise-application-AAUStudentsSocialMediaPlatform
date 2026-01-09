using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Auth.Domain.Events;

public record UserRegisteredDomainEvent(Guid UserId, string Email) : IDomainEvent;
