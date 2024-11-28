using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.GetProjectsByUser;

public record GetProjectsByUserCommand(int UserId, int PageNumber, int PageSize) : IRequest<ResultResponse<PagedResult<GetProjectResult>>>
{

    public GetProjectsByUserCommand() : this(0,0,0)
    {
    }

    public static GetProjectsByUserCommand Create(int userId, int pageNumber, int pageSize)
    {
        return new GetProjectsByUserCommand(userId, pageNumber, pageSize);
    }
}