using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Timeline.Application.Queries;
using AAU.Connect.Modules.Timeline.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Timeline.Infrastructure.Handlers;

public class GetCommentsQueryHandler(TimelineDbContext context) : IQueryHandler<GetCommentsQuery, List<CommentDto>>
{
    public async Task<List<CommentDto>> Handle(GetCommentsQuery query, CancellationToken cancellationToken)
    {
        return await context.Comments
            .Where(c => c.PostId == query.PostId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto(c.Id, c.UserId, c.Content, c.CreatedAt.GetValueOrDefault()))
            .ToListAsync(cancellationToken);
    }
}

