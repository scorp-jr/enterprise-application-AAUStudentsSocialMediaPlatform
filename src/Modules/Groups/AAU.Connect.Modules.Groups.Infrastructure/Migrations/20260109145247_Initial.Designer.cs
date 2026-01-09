
using System;
using AAU.Connect.Modules.Groups.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AAU.Connect.Modules.Groups.Infrastructure.Migrations
{
    [DbContext(typeof(GroupsDbContext))]
    [Migration("20260109145247_Initial")]
    partial class Initial
    {
        
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("groups")
                .HasAnnotation("ProductVersion", "10.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AAU.Connect.BuildingBlocks.Domain.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", "groups");
                });

            modelBuilder.Entity("AAU.Connect.Modules.Groups.Domain.Aggregates.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BannerUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.PrimitiveCollection<Guid[]>("MemberIds")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Groups", "groups");
                });

            modelBuilder.Entity("AAU.Connect.Modules.Groups.Domain.Aggregates.Group", b =>
                {
                    b.OwnsMany("AAU.Connect.Modules.Groups.Domain.Entities.Assignment", "Assignments", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<DateTime?>("CreatedAt")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("CreatedBy")
                                .HasColumnType("text");

                            b1.Property<DateTime?>("Deadline")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<Guid>("GroupId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime?>("LastModified")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("LastModifiedBy")
                                .HasColumnType("text");

                            b1.Property<int>("Status")
                                .HasColumnType("integer");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)");

                            b1.HasKey("Id");

                            b1.HasIndex("GroupId");

                            b1.ToTable("Assignments", "groups");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });

                    b.OwnsMany("AAU.Connect.Modules.Groups.Domain.Entities.Resource", "Resources", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<DateTime?>("CreatedAt")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("CreatedBy")
                                .HasColumnType("text");

                            b1.Property<Guid>("GroupId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime?>("LastModified")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("LastModifiedBy")
                                .HasColumnType("text");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)");

                            b1.Property<int>("Type")
                                .HasColumnType("integer");

                            b1.Property<string>("Url")
                                .IsRequired()
                                .HasMaxLength(1000)
                                .HasColumnType("character varying(1000)");

                            b1.HasKey("Id");

                            b1.HasIndex("GroupId");

                            b1.ToTable("Resources", "groups");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });

                    b.Navigation("Assignments");

                    b.Navigation("Resources");
                });
#pragma warning restore 612, 618
        }
    }
}
