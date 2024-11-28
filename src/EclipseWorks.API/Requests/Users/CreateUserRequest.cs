namespace EclipseWorks.API.Requests.Users;

public record CreateUserRequest(string Username)
{
    public CreateUserRequest() : this(string.Empty)
    {
    }

    public static CreateUserRequest Create(string username) => new(username);
}