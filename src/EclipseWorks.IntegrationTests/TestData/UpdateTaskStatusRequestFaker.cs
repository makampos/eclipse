using Bogus;
using EclipseWorks.API.Requests.Tasks;
using EclipseWorks.Domain.Enum;

namespace EclipseWorks.IntegrationTests.TestData;

public class UpdateTaskStatusRequestFaker
{
    public static readonly Faker<UpdateTaskStatusRequest> _updateTaskStatusRequestFaker = new
            Faker<UpdateTaskStatusRequest>()
        .RuleFor(x => x.Status, f => f.PickRandom<Status>());

    public static UpdateTaskStatusRequest GenerateValidaRequest(int id, int userId, Status status)
    {
        return _updateTaskStatusRequestFaker
            .RuleFor(x => x.Id, f => id)
            .RuleFor(x => x.UserId, f => userId)
            .RuleFor(x => x.Status, f => status)
            .Generate();
    }
}