using EclipseWorks.Domain.Enum;

namespace EclipseWorks.API.Requests.Users;

public record CreateUserRequest(string Username, Role Role)
{
    public CreateUserRequest() : this(string.Empty, Role.Undefined)
    {
    }

    public static CreateUserRequest Create(string username, Role role) => new(username, role);
}