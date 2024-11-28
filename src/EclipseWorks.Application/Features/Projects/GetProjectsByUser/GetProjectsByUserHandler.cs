using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.GetProjectsByUser;

public class GetProjectsByUserHandler : IRequestHandler<GetProjectsByUserCommand, ResultResponse<PagedResult<GetProjectResult>>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<GetProjectsByUserHandler> _logger;

    public GetProjectsByUserHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<GetProjectsByUserHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<PagedResult<GetProjectResult>>> Handle(GetProjectsByUserCommand command, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {GetProjectsByUserHandler} triggered to handle {GetProjectsByUserCommand}",
            nameof(GetProjectsByUserHandler), command.UserId);

        var pagedResult = await _eclipseUnitOfWork.ProjectRepository.GetProjectsByUserAsync(command.PageNumber,
            command.PageSize, command.UserId, cancellationToken);

        var getProjectsResult = GetProjectsByUserMap.MapToGetProjectsResult(pagedResult.Items);

        var nPagedResult = PagedResult<GetProjectResult>.Create(
            getProjectsResult,
            pagedResult.TotalCount,
            pagedResult.PageSize,
            pagedResult.CurrentPage);

        return ResultResponse<PagedResult<GetProjectResult>>.SuccessResult(nPagedResult);
    }
}