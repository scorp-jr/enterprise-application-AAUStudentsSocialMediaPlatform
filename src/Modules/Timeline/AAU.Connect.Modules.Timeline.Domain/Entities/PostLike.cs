using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Timeline.Domain.Entities;

public class PostLike : Entity
{
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }

    private PostLike() { } 

    public static PostLike Create(Guid postId, Guid userId)
    {
        return new PostLike
        {
            Id = Guid.NewGuid(),
            PostId = postId,
            UserId = userId
        };
    }
}
