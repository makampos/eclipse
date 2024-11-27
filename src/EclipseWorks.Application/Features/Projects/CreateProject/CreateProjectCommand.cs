using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.CreateProject;

public record CreateProjectCommand(
    string Name,
    string Description) : IRequest<ResultResponse<CreateProjectResult>>
{
    public CreateProjectCommand() : this(string.Empty, string.Empty)
    {

    }
    public static CreateProjectCommand Create(string name, string description)
    {
        return new CreateProjectCommand(name, description);
    }
}