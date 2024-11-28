namespace EclipseWorks.Application.Features.Users.CreateUser;

public record CreateUserResult(int Id)
{
    public CreateUserResult() : this(0) { }
    public static CreateUserResult Create(int id) => new(id);
}