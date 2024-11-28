using Bogus;
using EclipseWorks.Application.Features.CreateProject;

namespace EclipseWorks.UnitTests.Features.TestData;

public static class CreateProjectHandlerFaker
{
    private static readonly Faker<CreateProjectCommand> _createProjectCommandFaker = new Faker<CreateProjectCommand>()
        .RuleFor(x => x.Name, f => f.Company.CompanyName())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence())
        .RuleFor(x => x.UserId, f => f.Random.Int());

    public static CreateProjectCommand GenerateValidCommand(int userId)
    {
        return _createProjectCommandFaker
            .RuleFor(x => x.UserId, f => userId)
            .Generate();

    }

    public static List<CreateProjectCommand> GenerateValidCommands(int userId, int count)
    {
        return _createProjectCommandFaker
            .RuleFor(x => x.UserId, f => userId)
            .Generate(count);
    }
}