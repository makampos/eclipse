using Bogus;
using EclipseWorks.API.Requests.Projects;

namespace EclipseWorks.IntegrationTests.TestData;

public static class CreateProjectRequestFaker
{
    private readonly static Faker<CreateProjectRequest> _createProjectRequestFaker = new Faker<CreateProjectRequest>()
        .RuleFor(x => x.Name, f => f.Company.CompanyName())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence());

    public static CreateProjectRequest GenerateValidRequest(int userId)
    {
        return _createProjectRequestFaker
            .RuleFor(x => x.UserId, _ => userId)
            .Generate();
    }
}