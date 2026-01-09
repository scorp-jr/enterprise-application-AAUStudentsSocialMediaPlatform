using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Timeline.Domain.ValueObjects;
using MediatR;

namespace AAU.Connect.Modules.Timeline.Application.Commands;

public record CreatePostCommand(
    Guid UserId,
    string Caption,
    string MediaUrl,
    ImageFilters Filters,
    string? Location,
    List<string> Tags) : ICommand<Guid>;
