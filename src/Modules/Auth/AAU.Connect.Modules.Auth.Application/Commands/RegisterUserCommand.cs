using AAU.Connect.BuildingBlocks.Application;
namespace AAU.Connect.Modules.Auth.Application.Commands;
public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : ICommand<Guid>;
