namespace AAU.Connect.BuildingBlocks.Domain
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; protected set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
