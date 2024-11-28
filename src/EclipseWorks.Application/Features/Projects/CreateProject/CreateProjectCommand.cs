using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.CreateProject;

public record CreateProjectCommand(
    string Name,
    string Description,
    int UserId) : IRequest<ResultResponse<CreateProjectResult>>
{
    public CreateProjectCommand() : this(string.Empty, string.Empty, 0)
    {

    }
    public static CreateProjectCommand Create(string name, string description, int userId)
    {
        return new CreateProjectCommand(name, description, userId);
    }
}