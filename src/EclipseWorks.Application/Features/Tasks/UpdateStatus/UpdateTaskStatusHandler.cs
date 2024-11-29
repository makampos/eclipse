using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Task = EclipseWorks.Domain.Models.Task;

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

        var taskHistory = TaskHistory.Create(
            taskId: task.Id,
            taskAction: TaskAction.Modified,
            userId: statusCommand.UserId,
            affectedProperties: nameof(Task.Status)
        );

        task.AddHistory(taskHistory);

        await _eclipseUnitOfWork.TaskRepository.UpdateAsync(task, cancellationToken);
        await _eclipseUnitOfWork.SaveChangesAsync(cancellationToken);

        return ResultResponse<UpdateTaskStatusResult>.SuccessResult(UpdateTaskStatusResult.Create("Task completed successfully"));
    }
}