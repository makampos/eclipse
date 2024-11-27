using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.Tasks.DeleteTask;

public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, ResultResponse<DeleteTaskResult>>
{
    private readonly IEclipseUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTaskHandler> _logger;

    public DeleteTaskHandler(IEclipseUnitOfWork unitOfWork, ILogger<DeleteTaskHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<DeleteTaskResult>> Handle(DeleteTaskCommand command, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {DeleteTaskHandler} triggered to handle {DeleteTaskCommand}", nameof(DeleteTaskHandler), command);

        var task = await _unitOfWork.TaskRepository.GetByIdAsync(command.Id, cancellationToken);

        if (task is null)
        {
            _logger.LogWarning("Task with id: {Id} not found", command.Id);
            return ResultResponse<DeleteTaskResult>.FailureResult($"Task with id: {command.Id} not found");
        }

        await _unitOfWork.TaskRepository.DeleteAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultResponse<DeleteTaskResult>.SuccessResult(DeleteTaskResult.Create($"Task with name: {task.Name} has been deleted successfully"));
    }
}