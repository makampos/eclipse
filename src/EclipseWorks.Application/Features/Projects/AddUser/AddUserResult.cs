namespace EclipseWorks.Application.Features.AddUser;

public record AddUserResult(string Message)
{
    public static AddUserResult Create(string message) => new(message);
}