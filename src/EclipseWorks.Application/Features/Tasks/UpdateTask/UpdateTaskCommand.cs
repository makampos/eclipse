using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Tasks.UpdateTask;

public record UpdateTaskCommand(
    int Id,
    string Name,
    string Description) : IRequest<ResultResponse<UpdateTaskResult>>
{
    public UpdateTaskCommand() : this(0, string.Empty, string.Empty) { }

    public static UpdateTaskCommand Create(int id, string name, string description)
    {
        return new UpdateTaskCommand(id, name, description);
    }
}