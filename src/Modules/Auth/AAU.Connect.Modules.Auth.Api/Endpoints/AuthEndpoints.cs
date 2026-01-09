using AAU.Connect.Modules.Auth.Application.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AAU.Connect.Modules.Auth.Api.Endpoints
{
    public class AuthEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var folder = app.MapGroup("/auth");

            folder.MapPost("/register", async (RegisterUserCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("RegisterUser")
            .WithOpenApi();

            folder.MapGet("/me", (HttpContext context) =>
            {
                var user = context.User;
                return Results.Ok(new
                {
                    IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
                    Name = user.Identity?.Name,
                    Claims = user.Claims.Select(c => new { c.Type, c.Value })
                });
            })
            .RequireAuthorization()
            .WithName("GetMe")
            .WithOpenApi();
        }
    }
}
