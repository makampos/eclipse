using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.GetProject;

public record GetProjectCommand(int Id) : IRequest<ResultResponse<GetProjectResult>>
{
    public static GetProjectCommand Create(int id) => new(id);
}