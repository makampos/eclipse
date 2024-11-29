using EclipseWorks.Application.Specifications;
using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.Tasks.CreateTask;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, ResultResponse<CreateTaskResult>>
{
    private readonly IEclipseUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTaskHandler> _logger;

    public CreateTaskHandler(IEclipseUnitOfWork unitOfWork, ILogger<CreateTaskHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<CreateTaskResult>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {CreateTaskHandler} triggered to handle {CreateTaskCommand}",
            nameof(CreateTaskHandler), command);

        var project = await _unitOfWork.ProjectRepository.GetByIdIncludeAsync(command.ProjectId,
            query => query.Include(p => p.Tasks), cancellationToken);

        if (project is null)
        {
            _logger.LogError("Project with id {ProjectId} not found.", command.ProjectId);
            return ResultResponse<CreateTaskResult>.FailureResult($"Project with id {command.ProjectId} not found");
        }

        var maxTaxSpecification = new MaxTasksSpecification(20);

        if (maxTaxSpecification.IsSatisfiedBy(project))
        {
            _logger.LogError("Project with id {ProjectId} has reached the maximum number of tasks.", command.ProjectId);
            return ResultResponse<CreateTaskResult>.FailureResult(
                $"Project with id {command.ProjectId} has reached the maximum number of tasks");
        }

        var task = command.MapToEntity();

        var taskHistory = TaskHistory.Create(
            taskId: task.Id,
            command.UserId,
            taskAction: TaskAction.Created);
        var taskUser = TaskUser.Create(command.UserId, task.Id);

        task.AddHistory(taskHistory);
        task.AddTaskUser(taskUser);

        await _unitOfWork.TaskRepository.AddAsync(task, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = task.MapToCreateTaskResult();
        return ResultResponse<CreateTaskResult>.SuccessResult(result);
    }
}