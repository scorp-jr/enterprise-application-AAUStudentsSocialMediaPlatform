using AAU.Connect.Modules.Auth.Domain.Aggregates;
using AAU.Connect.Modules.Auth.Domain.Interfaces;
using AAU.Connect.Modules.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AAU.Connect.Modules.Auth.Infrastructure.Repositories;

public class UserRepository(AuthDbContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
