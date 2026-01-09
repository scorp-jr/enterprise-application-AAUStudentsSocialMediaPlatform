using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AAU.Connect.Modules.Groups.Domain.Aggregates;
using AAU.Connect.Modules.Groups.Domain.Entities;

namespace AAU.Connect.Modules.Groups.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name).HasMaxLength(255).IsRequired();
        builder.Property(g => g.Description).HasMaxLength(2000);
        builder.Property(g => g.BannerUrl).HasMaxLength(500);

        builder.Property(g => g.MemberIds);

        builder.OwnsMany(g => g.Assignments, a =>
        {
            a.ToTable("Assignments");
            a.WithOwner().HasForeignKey("GroupId");
            a.HasKey(x => x.Id);
            a.Property(x => x.Title).HasMaxLength(500).IsRequired();
        });

        builder.OwnsMany(g => g.Resources, r =>
        {
            r.ToTable("Resources");
            r.WithOwner().HasForeignKey("GroupId");
            r.HasKey(x => x.Id);
            r.Property(x => x.Title).HasMaxLength(500).IsRequired();
            r.Property(x => x.Url).HasMaxLength(1000).IsRequired();
        });

        builder.Ignore(g => g.DomainEvents);
    }
}
