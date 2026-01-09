using AAU.Connect.Modules.Groups.Application.Commands;
using AAU.Connect.Modules.Groups.Domain.Aggregates;
using AAU.Connect.Modules.Groups.Domain.ValueObjects;
using AAU.Connect.Modules.Groups.Domain.Interfaces;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AAU.Connect.Modules.Groups.Api.Endpoints;

public class GroupsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var folder = app.MapGroup("/groups")
            .WithTags("Groups");

        folder.MapPost("/", async (CreateGroupRequest request, ISender sender) =>
        {
            var command = new CreateGroupCommand(
                request.Name,
                request.Description,
                request.Type,
                request.CreatorId,
                request.BannerUrl);

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .RequireAuthorization(AAU.Connect.BuildingBlocks.Infrastructure.Authorization.AuthorizationPolicies.RequireInstructorRole)
        .WithName("CreateGroup")
        .WithOpenApi();

        folder.MapGet("/", async (IGroupRepository repository, [FromQuery] GroupType? type) =>
        {
            var groups = await repository.GetAllAsync(type);
            return Results.Ok(groups);
        })
        .WithName("GetGroups")
        .WithOpenApi();

        folder.MapGet("/{id}", async (Guid id, IGroupRepository repository) =>
        {
            var group = await repository.GetByIdAsync(id);
            return group is not null ? Results.Ok(group) : Results.NotFound();
        })
        .WithName("GetGroupById")
        .WithOpenApi();
    }
}

public record CreateGroupRequest(
    string Name,
    string Description,
    GroupType Type,
    Guid CreatorId,
    string? BannerUrl);
