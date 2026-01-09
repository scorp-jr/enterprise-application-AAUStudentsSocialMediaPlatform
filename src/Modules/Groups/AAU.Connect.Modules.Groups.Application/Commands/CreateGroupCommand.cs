using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Groups.Domain.ValueObjects;

namespace AAU.Connect.Modules.Groups.Application.Commands;

public record CreateGroupCommand(
    string Name,
    string Description,
    GroupType Type,
    Guid CreatorId,
    string? BannerUrl) : ICommand<Guid>;
