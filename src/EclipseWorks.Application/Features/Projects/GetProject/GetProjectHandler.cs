using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.GetProject;

public class GetProjectHandler : IRequestHandler<GetProjectCommand, ResultResponse<GetProjectResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<GetProjectHandler> _logger;

    public GetProjectHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<GetProjectHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<GetProjectResult>> Handle(GetProjectCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetProjectHandler} triggered to handle {GetProjectCommand}",
            nameof(GetProjectHandler), command);

        var project = await _eclipseUnitOfWork.ProjectRepository.GetByIdIncludeNonDeletedTaskAsync(command.Id,
            query => query.Include(x => x.Tasks), cancellationToken);

        if (project is null)
        {
            _logger.LogWarning("Project with id {Id} not found", command.Id);
            return ResultResponse<GetProjectResult>.FailureResult($"Project with id: {command.Id} not found");
        }

        var result = project.ToGetProjectResult();
        return ResultResponse<GetProjectResult>.SuccessResult(result);
    }
}