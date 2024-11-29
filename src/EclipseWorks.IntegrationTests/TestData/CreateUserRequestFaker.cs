using Bogus;
using EclipseWorks.API.Requests.Users;
using EclipseWorks.Domain.Enum;

namespace EclipseWorks.IntegrationTests.TestData;

public class CreateUserRequestFaker
{
    public static readonly Faker<CreateUserRequest> _createUserRequest = new Faker<CreateUserRequest>()
        .RuleFor(x => x.Username, f => f.Name.FirstName())
        .RuleFor(x => x.Role, f => f.PickRandom<Role>());

    public static CreateUserRequest GenerateValidRequest(Role? role = null)
    {
        if (role is not null)
        {
            return _createUserRequest
                .RuleFor(x => x.Role, _ => role.Value)
                .Generate();
        }

        return _createUserRequest.Generate();
    }
}