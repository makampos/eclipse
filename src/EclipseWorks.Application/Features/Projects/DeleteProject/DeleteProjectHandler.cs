using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, ResultResponse<DeleteProjectResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<DeleteProjectHandler> _logger;

    public DeleteProjectHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<DeleteProjectHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<DeleteProjectResult>> Handle(DeleteProjectCommand command, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {DeleteProjectHandler} triggered to handle {DeleteProjectCommand}",
          nameof(DeleteProjectHandler), command);

        var project = await _eclipseUnitOfWork.ProjectRepository.GetByIdAsync(command.Id, cancellationToken);

        if (project is null)
        {
            _logger.LogWarning("Project with id {Id} not found", command.Id);
            return ResultResponse<DeleteProjectResult>.FailureResult($"Project with id: {command.Id} not found");
        }

        if (project.Tasks.Any(x => !x.IsCompleted))
        {
            _logger.LogWarning("Project with id {Id} has incomplete tasks", command.Id);
            return ResultResponse<DeleteProjectResult>.FailureResult(
                "Project has incomplete tasks, please complete all tasks or delete them first");
        }

        await _eclipseUnitOfWork.ProjectRepository.DeleteAsync(project, cancellationToken);
        await _eclipseUnitOfWork.TaskRepository.DeleteRangeAsync(project.Tasks, cancellationToken);
        await _eclipseUnitOfWork.SaveChangesAsync(cancellationToken);

        var result = DeleteProjectResult.Create($"Project with name: {project.Name} has been deleted successfully");

        return ResultResponse<DeleteProjectResult>.SuccessResult(result);
    }
}