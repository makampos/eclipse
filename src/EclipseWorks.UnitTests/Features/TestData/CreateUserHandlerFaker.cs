using Bogus;
using EclipseWorks.Application.Features.Users.CreateUser;

namespace EclipseWorks.UnitTests.Features.TestData;

public class CreateUserHandlerFaker
{
    public static readonly Faker<CreateUserCommand> _userFaker = new Faker<CreateUserCommand>()
        .RuleFor(u => u.Username, f => f.Name.FirstName());
    public static CreateUserCommand GenerateValidCommand()
    {
        return _userFaker.Generate();
    }
}