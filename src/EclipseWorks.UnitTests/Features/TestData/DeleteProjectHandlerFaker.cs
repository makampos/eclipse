using Bogus;
using EclipseWorks.Application.Features.DeleteProject;

namespace EclipseWorks.UnitTests.Features.TestData;

public class DeleteProjectHandlerFaker
{
    private static readonly Faker<DeleteProjectCommand> _deleteProjectCommandFaker = new Faker<DeleteProjectCommand>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 1000));

    public static DeleteProjectCommand GenerateValidCommand(int projectId)
    {
        return _deleteProjectCommandFaker
            .RuleFor(x => x.Id, f => projectId)
            .Generate();
    }
}