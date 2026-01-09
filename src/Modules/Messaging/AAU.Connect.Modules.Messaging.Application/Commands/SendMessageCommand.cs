using AAU.Connect.BuildingBlocks.Application;

namespace AAU.Connect.Modules.Messaging.Application.Commands;

public record SendMessageCommand(Guid ConversationId, Guid SenderId, string Content) : ICommand<Guid>;
