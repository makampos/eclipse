using Bogus;
using EclipseWorks.Application.Features.Tasks.UpdateTask;

namespace EclipseWorks.UnitTests.Features.TestData;

public class UpdateTaskHandlerFaker
{
    public static readonly Faker<UpdateTaskCommand> _updateTaskCommandFaker = new Faker<UpdateTaskCommand>()
        .RuleFor(x => x.Id, f => f.Random.Number(1, 100))
        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence())
        .RuleFor(x => x.DueDate, f => DateOnly.FromDateTime(f.Date.Future()));

    public static UpdateTaskCommand GenerateValidCommand(int taskId)
    {
        return _updateTaskCommandFaker
            .RuleFor(x => x.Id, _ => taskId)
            .Generate();
    }
}