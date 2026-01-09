using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Messaging.Domain.Entities;

public class Message : Entity
{
    public Guid ConversationId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime SentAt { get; private set; }

    private Message() { }

    public Message(Guid conversationId, Guid senderId, string content)
    {
        Id = Guid.NewGuid();
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        SentAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }
}
