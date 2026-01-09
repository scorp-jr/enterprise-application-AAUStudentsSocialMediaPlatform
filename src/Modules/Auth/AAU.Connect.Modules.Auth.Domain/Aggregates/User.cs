using AAU.Connect.BuildingBlocks.Domain;
using AAU.Connect.Modules.Auth.Domain.Events;

namespace AAU.Connect.Modules.Auth.Domain.Aggregates;

public class User : AggregateRoot
{
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;

    private User() { }

    public static User Create(Guid id, string email, string firstName, string lastName, string role)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required");
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First Name is required");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last Name is required");

        var user = new User
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id, user.Email));

        return user;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
         if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First Name is required");
         if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last Name is required");

         FirstName = firstName;
         LastName = lastName;
    }
}
