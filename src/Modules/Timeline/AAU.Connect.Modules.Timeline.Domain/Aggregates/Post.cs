using AAU.Connect.BuildingBlocks.Domain;
using AAU.Connect.Modules.Timeline.Domain.Events;
using AAU.Connect.Modules.Timeline.Domain.ValueObjects;

namespace AAU.Connect.Modules.Timeline.Domain.Aggregates;

public class Post : AggregateRoot
{
    public Guid UserId { get; private set; }
    public string Caption { get; private set; } = string.Empty;
    public string MediaUrl { get; private set; } = string.Empty;
    public ImageFilters Filters { get; private set; } = ImageFilters.Default;
    public string? Location { get; private set; }
    public List<string> Tags { get; private set; } = new();

    private Post() { } 

    private Post(Guid userId, string caption, string mediaUrl, ImageFilters filters, string? location, List<string> tags)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Caption = caption;
        MediaUrl = mediaUrl;
        Filters = filters;
        Location = location;
        Tags = tags;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new PostCreatedDomainEvent(Id, UserId));
    }

    public static Post Create(Guid userId, string caption, string mediaUrl, ImageFilters filters, string? location, List<string> tags)
    {
        if (string.IsNullOrWhiteSpace(mediaUrl))
            throw new ArgumentException("Media URL is required", nameof(mediaUrl));

        return new Post(userId, caption, mediaUrl, filters, location, tags);
    }

    public void UpdateDetails(string caption, string? location, List<string> tags)
    {
        Caption = caption;
        Location = location;
        Tags = tags;
        LastModified = DateTime.UtcNow;
    }

    public void ApplyFilters(ImageFilters filters)
    {
        Filters = filters;
        LastModified = DateTime.UtcNow;
    }
}
