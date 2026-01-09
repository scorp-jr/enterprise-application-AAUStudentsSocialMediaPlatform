using AAU.Connect.Modules.Auth.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AAU.Connect.BuildingBlocks.Domain.Outbox;
using AAU.Connect.BuildingBlocks.Application.Common.Interfaces;

namespace AAU.Connect.Modules.Auth.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext, IOutboxDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("auth");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
