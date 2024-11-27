using Bogus;
using EclipseWorks.Application.Features.Tasks.CreateTask;

namespace EclipseWorks.UnitTests.Features.TestData;

public static class CreateTaskHandlerFaker
{
    private readonly static Faker<CreateTaskCommand> _createTaskCommandFaker = new Faker<CreateTaskCommand>()
        .RuleFor(x => x.Name, f => f.Lorem.Word())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence())
        .RuleFor(x => x.ProjectId, f => f.Random.Int(1, 1000));

    public static CreateTaskCommand GenerateValidCommand(int projectId)
    {
        return _createTaskCommandFaker
            .RuleFor(x => x.ProjectId, f => projectId);
    }

    public static IEnumerable<CreateTaskCommand> GenerateValidCommands(int count, int projectId)
    {
        return _createTaskCommandFaker
            .RuleFor(x => x.ProjectId, f => projectId)
            .Generate(count);
    }
}