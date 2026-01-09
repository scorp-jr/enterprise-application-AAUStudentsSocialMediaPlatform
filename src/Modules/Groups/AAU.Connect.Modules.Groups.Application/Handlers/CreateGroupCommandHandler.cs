using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Groups.Domain.Aggregates;
using AAU.Connect.Modules.Groups.Domain.Interfaces;
using AAU.Connect.Modules.Groups.Application.Commands;

namespace AAU.Connect.Modules.Groups.Application.Handlers;

public class CreateGroupCommandHandler(IGroupRepository groupRepository) : ICommandHandler<CreateGroupCommand, Guid>
{
    public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = Group.Create(
            request.Name,
            request.Description,
            request.Type,
            request.CreatorId,
            request.BannerUrl);

        await groupRepository.AddAsync(group, cancellationToken);
        
        return group.Id;
    }
}
