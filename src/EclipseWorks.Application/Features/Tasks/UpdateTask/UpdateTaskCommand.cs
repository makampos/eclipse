using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Tasks.UpdateTask;

public record UpdateTaskCommand(
    int Id,
    string Name,
    string Description,
    int UserId,
    DateOnly DueDate) : IRequest<ResultResponse<UpdateTaskResult>>
{
    public UpdateTaskCommand() : this(0, string.Empty, string.Empty,0, DateOnly.MinValue) { }

    public static UpdateTaskCommand Create(int id, string name, string description, int userId, DateOnly dueDate)
    {
        return new UpdateTaskCommand(id, name, description, userId, dueDate);
    }
}