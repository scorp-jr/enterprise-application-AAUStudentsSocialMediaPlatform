namespace AAU.Connect.BuildingBlocks.Domain
{
    public interface IEntity
    {
        public Guid Id { get; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
