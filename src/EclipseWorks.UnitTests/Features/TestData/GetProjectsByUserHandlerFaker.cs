using Bogus;
using EclipseWorks.Application.Features.GetProjectsByUser;

namespace EclipseWorks.UnitTests.Features.TestData;

public class GetProjectsByUserHandlerFaker
{
    public static readonly Faker<GetProjectsByUserCommand> _getProjectsByUserCommandFaker = new
            Faker<GetProjectsByUserCommand>()
        .RuleFor(x => x.UserId, f => f.Random.Int());

    public static GetProjectsByUserCommand GenerateValidCommand(int userId, int pageNumber, int pageSize)
    {
        return _getProjectsByUserCommandFaker
            .RuleFor(x => x.UserId, f => userId)
            .RuleFor(x => x.PageNumber, f => pageNumber)
            .RuleFor(x => x.PageSize, f => pageSize)
            .Generate();
    }
}