using AAU.Connect.BuildingBlocks.Application;

namespace AAU.Connect.Modules.Timeline.Application.Queries;

public record GetCommentsQuery(Guid PostId) : IQuery<List<CommentDto>>;
public record CommentDto(Guid Id, Guid UserId, string Content, DateTime CreatedAt);

