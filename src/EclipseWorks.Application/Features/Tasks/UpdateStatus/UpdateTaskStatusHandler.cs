using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.Tasks.UpdateStatus;

public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskStatusCommand, ResultResponse<UpdateTaskStatusResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<UpdateTaskStatusHandler> _logger;

    public UpdateTaskStatusHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<UpdateTaskStatusHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<UpdateTaskStatusResult>> Handle(UpdateTaskStatusCommand statusCommand, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {UpdateTaskStatusHandler} triggered to handle {UpdateTaskStatusCommand}",
            nameof(UpdateTaskStatusHandler), statusCommand);

        var task = await _eclipseUnitOfWork.TaskRepository.GetByIdAsync(statusCommand.Id, cancellationToken);

        if (task is null)
        {
            _logger.LogWarning("Task with id {Id} not found", statusCommand.Id);
            return ResultResponse<UpdateTaskStatusResult>.FailureResult($"Task with id: {statusCommand.Id} not found");
        }

        task.UpdateStatus(statusCommand.Status);

        await _eclipseUnitOfWork.SaveChangesAsync(cancellationToken);

        return ResultResponse<UpdateTaskStatusResult>.SuccessResult(UpdateTaskStatusResult.Create("Task completed successfully"));
    }
}