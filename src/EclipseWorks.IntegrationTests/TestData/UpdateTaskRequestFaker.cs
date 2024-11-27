using Bogus;
using EclipseWorks.API.Requests.Tasks;

namespace EclipseWorks.IntegrationTests.TestData;
public class UpdateTaskRequestFaker
{
    public static readonly Faker<UpdateTaskRequest> _updateTaskRequestFaker = new Faker<UpdateTaskRequest>()
        .RuleFor(x => x.Name, f => f.Lorem.Sentence())
        .RuleFor(x => x.Description, f => f.Lorem.Sentence());

    public static UpdateTaskRequest GenerateValidRequest()
    {
        return _updateTaskRequestFaker.Generate();
    }
}
