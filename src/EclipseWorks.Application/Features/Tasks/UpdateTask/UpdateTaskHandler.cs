using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.Tasks.UpdateTask;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, ResultResponse<UpdateTaskResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<UpdateTaskHandler> _logger;


    public UpdateTaskHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<UpdateTaskHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<UpdateTaskResult>> Handle(UpdateTaskCommand command, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {UpdateTaskHandler} triggered to handle {UpdateTaskCommand}",
            nameof(UpdateTaskHandler), command);

        var task = await _eclipseUnitOfWork.TaskRepository.GetByIdAsync(command.Id, cancellationToken);

        if (task is null)
        {
            _logger.LogError("Task with id {TaskId} not found.", command.Id);
            return ResultResponse<UpdateTaskResult>.FailureResult($"Task with id {command.Id} not found");
        }

        var affectedProperties = task.GetDifference(
            name: command.Name,
            description: command.Description,
            dueDate: command.DueDate);

        var taskHistory = TaskHistory.Create(
            taskId: task.Id,
            taskAction: TaskAction.Modified,
            userId: command.UserId,
            affectedProperties: affectedProperties
        );

        task.Update(command.Name, command.Description, command.DueDate);
        task.AddHistory(taskHistory);

        await _eclipseUnitOfWork.TaskRepository.UpdateAsync(task, cancellationToken);
        return ResultResponse<UpdateTaskResult>.SuccessResult(UpdateTaskResult.Create("Task updated successfully"));
    }
}