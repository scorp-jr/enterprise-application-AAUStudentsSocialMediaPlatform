using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Messaging.Domain.Aggregates;

public class Conversation : AggregateRoot
{
    public List<Guid> ParticipantIds { get; private set; } = new();
    public string LastMessagePreview { get; private set; } = string.Empty;
    public DateTime LastMessageAt { get; private set; }

    private Conversation() { } 

    public static Conversation Start(Guid initiatorId, Guid recipientId)
    {
        return new Conversation
        {
            Id = Guid.NewGuid(),
            ParticipantIds = new List<Guid> { initiatorId, recipientId },
            LastMessageAt = DateTime.UtcNow
        };
    }

    public void UpdateLastMessage(string content)
    {
        LastMessagePreview = content.Length > 50 ? content.Substring(0, 50) + "..." : content;
        LastMessageAt = DateTime.UtcNow;
    }
}
