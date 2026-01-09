using AAU.Connect.BuildingBlocks.Domain;

namespace AAU.Connect.Modules.Groups.Domain.Entities;

public class Assignment : Entity
{
    public string Title { get; private set; } = string.Empty;
    public DateTime? Deadline { get; private set; }
    public AssignmentStatus Status { get; private set; } = AssignmentStatus.ToDo;

    private Assignment() { }

    public Assignment(string title, DateTime? deadline)
    {
        Id = Guid.NewGuid();
        Title = title;
        Deadline = deadline;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(AssignmentStatus status)
    {
        Status = status;
        LastModified = DateTime.UtcNow;
    }
}

public enum AssignmentStatus
{
    ToDo,
    InProgress,
    Done
}
