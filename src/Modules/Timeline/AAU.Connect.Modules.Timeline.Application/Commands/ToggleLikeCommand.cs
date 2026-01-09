using AAU.Connect.BuildingBlocks.Application;

namespace AAU.Connect.Modules.Timeline.Application.Commands;

public record ToggleLikeCommand(Guid PostId, Guid UserId) : ICommand<bool>;
