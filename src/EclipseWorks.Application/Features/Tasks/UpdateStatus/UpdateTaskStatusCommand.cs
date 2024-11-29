using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Tasks.UpdateStatus;

public record UpdateTaskStatusCommand(int Id, Status Status, int UserId) :
    IRequest<ResultResponse<UpdateTaskStatusResult>>
{
    public UpdateTaskStatusCommand() : this(0, Status.None, 0) { }

    public static UpdateTaskStatusCommand Create(int id, Status status, int userId) => new UpdateTaskStatusCommand(id,
        status, userId);
}