using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Timeline.Domain.Entities;

public class Comment : AggregateRoot
{
    public Guid PostId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = string.Empty;

    private Comment() { } 

    public static Comment Create(Guid postId, Guid userId, string content)
    {
        return new Comment
        {
            Id = Guid.NewGuid(),
            PostId = postId,
            UserId = userId,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };
    }
}
