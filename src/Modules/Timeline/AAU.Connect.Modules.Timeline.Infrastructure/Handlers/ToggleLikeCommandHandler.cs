using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Timeline.Application.Commands;
using AAU.Connect.Modules.Timeline.Domain.Entities;
using AAU.Connect.Modules.Timeline.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Timeline.Infrastructure.Handlers;

public class ToggleLikeCommandHandler(TimelineDbContext context) : ICommandHandler<ToggleLikeCommand, bool>
{
    public async Task<bool> Handle(ToggleLikeCommand command, CancellationToken cancellationToken)
    {
        var existingLike = await context.PostLikes
            .FirstOrDefaultAsync(l => l.PostId == command.PostId && l.UserId == command.UserId, cancellationToken);

        if (existingLike is not null)
        {
            context.PostLikes.Remove(existingLike);
            await context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var newLike = PostLike.Create(command.PostId, command.UserId);
        context.PostLikes.Add(newLike);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

