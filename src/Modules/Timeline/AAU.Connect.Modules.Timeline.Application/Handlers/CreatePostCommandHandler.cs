using AAU.Connect.BuildingBlocks.Application;
using AAU.Connect.Modules.Timeline.Domain.Aggregates;
using AAU.Connect.Modules.Timeline.Domain.Interfaces;
using AAU.Connect.Modules.Timeline.Application.Commands;

namespace AAU.Connect.Modules.Timeline.Application.Handlers;


public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = Post.Create(
            request.UserId,
            request.Caption,
            request.MediaUrl,
            request.Filters,
            request.Location,
            request.Tags);

        await _postRepository.AddAsync(post, cancellationToken);
        
        return post.Id;
    }
}
