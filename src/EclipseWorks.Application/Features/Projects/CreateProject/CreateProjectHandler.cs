using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.CreateProject;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ResultResponse<CreateProjectResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<CreateProjectHandler> _logger;

    public CreateProjectHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<CreateProjectHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<CreateProjectResult>> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {CreateProjectHandler} triggered to handle {CreateProjectCommand}",
          nameof(CreateProjectHandler), command);

        var project = command.MapToEntity();

        await _eclipseUnitOfWork.ProjectRepository.AddAsync(project, cancellationToken);
        await _eclipseUnitOfWork.SaveChangesAsync(cancellationToken);

        var result = project.MapToCreateProjectResult();
        return ResultResponse<CreateProjectResult>.SuccessResult(result);
    }
}