using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Timeline.Application.Commands;
using AAU.Connect.Modules.Timeline.Domain.Entities;
using AAU.Connect.Modules.Timeline.Infrastructure.Persistence;

namespace AAU.Connect.Modules.Timeline.Infrastructure.Handlers;

public class AddCommentCommandHandler(TimelineDbContext context) : ICommandHandler<AddCommentCommand, Guid>
{
    public async Task<Guid> Handle(AddCommentCommand command, CancellationToken cancellationToken)
    {
        var comment = Comment.Create(command.PostId, command.UserId, command.Content);

        context.Comments.Add(comment);
        await context.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}

