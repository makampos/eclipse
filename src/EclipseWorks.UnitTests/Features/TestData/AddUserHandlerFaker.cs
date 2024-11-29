using Bogus;
using EclipseWorks.Application.Features.AddUser;

namespace EclipseWorks.UnitTests.Features.TestData;

public class AddUserHandlerFaker
{
    public static readonly Faker<AddUserCommand> _addUserCommandFaker = new Faker<AddUserCommand>()
        .RuleFor(x => x.ProjectId, f => f.Random.Int())
        .RuleFor(x => x.UserId, f => f.Random.Int());

    public static AddUserCommand GenerateValidCommand(int projectId, int userId)
    {
        return _addUserCommandFaker
            .RuleFor(x => x.ProjectId, f => projectId)
            .RuleFor(x => x.UserId, f => userId)
            .Generate();
    }

    public static List<AddUserCommand> GenerateValidCommands(int count, int projectId, int userId)
    {
        return _addUserCommandFaker
            .RuleFor(x => x.ProjectId, f => projectId)
            .RuleFor(x => x.UserId, f => userId)
            .Generate(count);
    }
}