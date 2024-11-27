using Bogus;
using EclipseWorks.API.Requests.Tasks;
using EclipseWorks.Domain.Enum;

namespace EclipseWorks.IntegrationTests.TestData;

public class CreateTaskRequestFaker
{
    private static readonly Faker<CreateTaskRequest> _createTaskRequestFaker = new Faker<CreateTaskRequest>()
        .RuleFor(x => x.Name, f => f.Company.CompanyName())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence())
        .RuleFor(x => x.PriorityLevel, f => f.PickRandom<PriorityLevel>())
        .RuleFor(x => x.ProjectId, f => f.Random.Number(1, 100));

    public static CreateTaskRequest GenerateValidRequest(int projectId)
    {
        return _createTaskRequestFaker
            .RuleFor(x => x.ProjectId, _ => projectId)
            .Generate();
    }

    public static IEnumerable<CreateTaskRequest> GenerateValidRequests(int count, int projectId)
    {
        return _createTaskRequestFaker
            .RuleFor(x => x.ProjectId, f => projectId)
            .Generate(count);
    }
}