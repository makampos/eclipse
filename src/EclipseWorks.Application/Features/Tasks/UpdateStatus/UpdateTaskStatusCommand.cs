using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Tasks.UpdateStatus;

public record UpdateTaskStatusCommand(int Id, bool Status) : IRequest<ResultResponse<UpdateTaskStatusResult>>
{
    public UpdateTaskStatusCommand() : this(0, false) { }

    public static UpdateTaskStatusCommand Create(int id, bool status) => new UpdateTaskStatusCommand(id, status);
}