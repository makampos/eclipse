using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.Tasks.DeleteTask;

public record DeleteTaskCommand(int Id) : IRequest<ResultResponse<DeleteTaskResult>>
{
    public static DeleteTaskCommand Create(int id) => new(id);
}