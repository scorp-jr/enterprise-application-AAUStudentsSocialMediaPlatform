using AAU.Connect.Modules.Messaging.Application.Queries;
using AAU.Connect.Modules.Messaging.Application.Commands;
using AAU.Connect.Modules.Messaging.Application.Queries;
using AAU.Connect.Modules.Messaging.Application.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AAU.Connect.Modules.Messaging.Api.Endpoints;

public class MessagingEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var folder = app.MapGroup("/messaging")
            .WithTags("Messaging");

        folder.MapPost("/conversations", async (StartConversationRequest request, ISender sender) =>
        {
            var command = new StartConversationCommand(request.InitiatorId, request.RecipientId);
            var result = await sender.Send(command);
            return Results.Ok(new { ConversationId = result });
        })
        .WithName("StartConversation")
        .WithOpenApi();

        folder.MapPost("/conversations/{conversationId}/messages", async (Guid conversationId, SendMessageRequest request, ISender sender) =>
        {
            var command = new SendMessageCommand(conversationId, request.SenderId, request.Content);
            var result = await sender.Send(command);
            return Results.Ok(new { MessageId = result });
        })
        .WithName("SendMessage")
        .WithOpenApi();

        folder.MapGet("/conversations", async ([AsParameters] GetConversationsRequest request, ISender sender) =>
        {
            var query = new GetConversationsQuery(request.UserId);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetConversations")
        .WithOpenApi();

        folder.MapGet("/conversations/{conversationId}/messages", async (Guid conversationId, ISender sender) =>
        {
            var query = new GetMessagesQuery(conversationId);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetMessages")
        .WithOpenApi();
    }
}

public record StartConversationRequest(Guid InitiatorId, Guid RecipientId);
public record SendMessageRequest(Guid SenderId, string Content);
public record GetConversationsRequest(Guid UserId);
