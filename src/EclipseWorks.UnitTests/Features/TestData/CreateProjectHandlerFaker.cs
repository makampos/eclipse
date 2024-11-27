using Bogus;
using EclipseWorks.Application.Features.CreateProject;

namespace EclipseWorks.UnitTests.Features.TestData;

public static class CreateProjectHandlerFaker
{
    private static readonly Faker<CreateProjectCommand> _createProjectCommandFaker = new Faker<CreateProjectCommand>()
        .RuleFor(x => x.Name, f => f.Company.CompanyName())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence());

    public static CreateProjectCommand GenerateValidCommand()
    {
        return _createProjectCommandFaker.Generate();
    }
}