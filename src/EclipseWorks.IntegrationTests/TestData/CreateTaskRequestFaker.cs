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
        .RuleFor(x => x.ProjectId, f => f.Random.Number(1, 100))
        .RuleFor(x => x.DueDate, f => DateOnly.FromDateTime(f.Date.Future()));

    public static CreateTaskRequest GenerateValidRequest(int projectId, int userId)
    {
        return _createTaskRequestFaker
            .RuleFor(x => x.ProjectId, _ => projectId)
            .RuleFor(x => x.UserId, _ => userId)
            .Generate();
    }

    public static IEnumerable<CreateTaskRequest> GenerateValidRequests(int count, int projectId, int userId, DateOnly
            dueDate)
    {
        return _createTaskRequestFaker
            .RuleFor(x => x.ProjectId, _ => projectId)
            .RuleFor(x => x.UserId, _ => userId)
            .RuleFor(x => x.DueDate, _ => dueDate)
            .Generate(count);
    }
}