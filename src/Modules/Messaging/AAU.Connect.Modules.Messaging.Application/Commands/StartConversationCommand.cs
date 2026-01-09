using AAU.Connect.BuildingBlocks.Application;

namespace AAU.Connect.Modules.Messaging.Application.Commands;

public record StartConversationCommand(Guid InitiatorId, Guid RecipientId) : ICommand<Guid>;
