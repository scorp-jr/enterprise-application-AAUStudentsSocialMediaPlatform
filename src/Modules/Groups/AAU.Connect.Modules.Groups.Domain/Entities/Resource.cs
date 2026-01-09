using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Groups.Domain.Entities;

public class Resource : Entity
{
    public string Title { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public ResourceType Type { get; private set; }

    private Resource() { }

    public Resource(string title, string url, ResourceType type)
    {
        Id = Guid.NewGuid();
        Title = title;
        Url = url;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }
}

public enum ResourceType
{
    PDF,
    Link,
    Video
}
