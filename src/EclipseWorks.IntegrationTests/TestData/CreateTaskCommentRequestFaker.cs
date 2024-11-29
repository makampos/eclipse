using Bogus;
using EclipseWorks.API.Requests.Tasks;

namespace EclipseWorks.IntegrationTests.TestData;

public class CreateTaskCommentRequestFaker
{
    public static readonly Faker<CreateCommentRequest> _createTaskCommentRequestFaker = new Faker<CreateCommentRequest>()
        .RuleFor(x => x.TaskId, f => f.Random.Number(1, 100))
        .RuleFor(x => x.UserId, f => f.Lorem.Random.Int())
        .RuleFor(x => x.Content, f => f.Lorem.Sentence());

    public static CreateCommentRequest GenerateValidRequest(int taskId, int userId)
    {
        return _createTaskCommentRequestFaker
            .RuleFor(x => x.TaskId, _ => taskId)
            .RuleFor(x => x.UserId, _ => userId);
    }

    public static List<CreateCommentRequest> GenerateValidRequests(int taskId, int userId, int count)
    {
        return _createTaskCommentRequestFaker
            .RuleFor(x => x.TaskId, _ => taskId)
            .RuleFor(x => x.UserId, _ => userId)
            .Generate(count);
    }
}