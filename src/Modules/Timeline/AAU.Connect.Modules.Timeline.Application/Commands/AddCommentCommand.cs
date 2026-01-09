using AAU.Connect.BuildingBlocks.Application;

namespace AAU.Connect.Modules.Timeline.Application.Commands;

public record AddCommentCommand(Guid PostId, Guid UserId, string Content) : ICommand<Guid>;
