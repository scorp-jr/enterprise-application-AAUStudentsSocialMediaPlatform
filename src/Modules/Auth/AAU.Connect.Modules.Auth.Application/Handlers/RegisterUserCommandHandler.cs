using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Auth.Application.Common.Interfaces;
using AAU.Connect.Modules.Auth.Domain.Aggregates;
using AAU.Connect.Modules.Auth.Domain.Interfaces;
using AAU.Connect.Modules.Auth.Application.Commands;

namespace AAU.Connect.Modules.Auth.Application.Handlers;

public class RegisterUserCommandHandler(IUserRepository userRepository, IIdentityService identityService) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var identityId = await identityService.CreateUserAsync(request.Email, request.Password, request.FirstName, request.LastName, cancellationToken);
        var user = User.Create(identityId, request.Email, request.FirstName, request.LastName, "Student");
        await userRepository.AddAsync(user, cancellationToken);
        return user.Id;
    }
}
