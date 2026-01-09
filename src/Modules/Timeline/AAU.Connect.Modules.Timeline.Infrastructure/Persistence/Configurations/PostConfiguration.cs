using AAU.Connect.Modules.Timeline.Domain.Aggregates;
using AAU.Connect.Modules.Timeline.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AAU.Connect.Modules.Timeline.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts", "timeline");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Filters)
            .HasConversion<int>();
    }
}
