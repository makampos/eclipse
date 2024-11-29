using Bogus;
using EclipseWorks.API.Requests.Projects;

namespace EclipseWorks.IntegrationTests.TestData;

public class AddUserRequestFaker
{
    public static Faker<AddUserRequest> _addUserRequestFaker = new Faker<AddUserRequest>()
        .RuleFor(x => x.ProjectId, f => f.Random.Int(1, 100))
        .RuleFor(x => x.UserId, f => f.Random.Int(1, 100));

    public static AddUserRequest GenerateValidRequest(int userId, int projectId)
    {
        return _addUserRequestFaker
            .RuleFor(x => x.UserId, f => userId)
            .RuleFor(x => x.ProjectId, f => projectId)
            .Generate();
    }
}