using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.DeleteProject;

public record DeleteProjectCommand(int Id) : IRequest<ResultResponse<DeleteProjectResult>>
{
    public DeleteProjectCommand() : this(0)
    {

    }
    public static DeleteProjectCommand Create(int id)
    {
        return new DeleteProjectCommand(id);
    }
}