using Bogus;
using EclipseWorks.API.Requests.Users;

namespace EclipseWorks.IntegrationTests.TestData;

public class CreateUserRequestFaker
{
    public static readonly Faker<CreateUserRequest> _createUserRequest = new Faker<CreateUserRequest>()
        .RuleFor(x => x.Username, f => f.Name.FirstName());

    public static CreateUserRequest GenerateValidRequest()
    {
        return _createUserRequest.Generate();
    }
}