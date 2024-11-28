using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Features.Users.CreateUser;

public static class UserMap
{
    public static User MapToEntity(this CreateUserCommand command)
    {
        return User.Create(command.Username);
    }

    public static CreateUserResult MapToCreateUserResult(this User user)
    {
        return CreateUserResult.Create(user.Id);
    }
}