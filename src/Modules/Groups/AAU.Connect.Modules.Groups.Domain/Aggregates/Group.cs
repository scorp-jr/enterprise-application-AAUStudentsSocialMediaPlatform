using AAU.Connect.Modules.Groups.Domain.Entities;
using AAU.Connect.Modules.Groups.Domain.ValueObjects;
using AAU.Connect.Modules.Groups.Domain.Events;
using AAU.Connect.BuildingBlocks.Domain;
using AAU.Connect.Modules.Groups.Domain.Events;

namespace AAU.Connect.Modules.Groups.Domain.Aggregates;

public class Group : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public GroupType Type { get; private set; }
    public string? BannerUrl { get; private set; }
    public Guid CreatorId { get; private set; }
    
    private readonly List<Guid> _memberIds = new();
    public IReadOnlyCollection<Guid> MemberIds => _memberIds.AsReadOnly();

    private readonly List<Assignment> _assignments = new();
    public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();

    private readonly List<Resource> _resources = new();
    public IReadOnlyCollection<Resource> Resources => _resources.AsReadOnly();

    private Group() { }

    public Group(string name, string description, GroupType type, Guid creatorId, string? bannerUrl)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Type = type;
        CreatorId = creatorId;
        BannerUrl = bannerUrl;
        CreatedAt = DateTime.UtcNow;
        _memberIds.Add(creatorId);

        AddDomainEvent(new GroupCreatedDomainEvent(Id, Name, CreatorId));
    }

    public static Group Create(string name, string description, GroupType type, Guid creatorId, string? bannerUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Group name is required", nameof(name));

        return new Group(name, description, type, creatorId, bannerUrl);
    }

    public void AddMember(Guid userId)
    {
        if (!_memberIds.Contains(userId))
            _memberIds.Add(userId);
    }

    public void AddAssignment(string title, DateTime? deadline)
    {
        if (Type == GroupType.Club)
            throw new InvalidOperationException("Clubs cannot have assignments.");

        _assignments.Add(new Assignment(title, deadline));
        LastModified = DateTime.UtcNow;
    }

    public void AddResource(string title, string url, ResourceType type)
    {
        _resources.Add(new Resource(title, url, type));
        LastModified = DateTime.UtcNow;
    }
}
