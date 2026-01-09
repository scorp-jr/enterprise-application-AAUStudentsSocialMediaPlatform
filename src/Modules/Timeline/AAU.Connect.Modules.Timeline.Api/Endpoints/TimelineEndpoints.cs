using AAU.Connect.Modules.Timeline.Application.Commands;
using AAU.Connect.Modules.Timeline.Application.Queries;
using AAU.Connect.Modules.Timeline.Domain.Aggregates;
using AAU.Connect.Modules.Timeline.Domain.Interfaces;
using AAU.Connect.Modules.Timeline.Domain.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Carter;
using MediatR;

namespace AAU.Connect.Modules.Timeline.Api.Endpoints;

public class TimelineEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var folder = app.MapGroup("/posts")
            .WithTags("Timeline");

        folder.MapPost("/", async (CreatePostRequest request, ISender sender) =>
        {
            var command = new CreatePostCommand(
                request.UserId,
                request.Caption,
                request.MediaUrl,
                request.Filters,
                request.Location,
                request.Tags);

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("CreatePost")
        .WithOpenApi();

        folder.MapGet("/", async (IPostRepository repository) =>
        {
            var posts = await repository.GetLatestPostsAsync();
            return Results.Ok(posts);
        })
        .WithName("GetFeed")
        .WithOpenApi();

        folder.MapPost("/{postId}/comments", async (Guid postId, AddCommentRequest request, ISender sender) =>
        {
            var command = new AddCommentCommand(postId, request.UserId, request.Content);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("AddComment")
        .WithOpenApi();

        folder.MapGet("/{postId}/comments", async (Guid postId, ISender sender) =>
        {
            var query = new GetCommentsQuery(postId);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetComments")
        .WithOpenApi();

        folder.MapPost("/{postId}/like", async (Guid postId, ToggleLikeRequest request, ISender sender) =>
        {
            var command = new ToggleLikeCommand(postId, request.UserId);
            var result = await sender.Send(command);
            return Results.Ok(new { IsLiked = result });
        })
        .WithName("ToggleLike")
        .WithOpenApi();
    }
}

public record CreatePostRequest(
    Guid UserId,
    string Caption,
    string MediaUrl,
    ImageFilters Filters,
    string? Location,
    List<string> Tags);

public record AddCommentRequest(Guid UserId, string Content);
public record ToggleLikeRequest(Guid UserId);
