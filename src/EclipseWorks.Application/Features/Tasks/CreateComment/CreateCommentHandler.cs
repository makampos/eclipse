using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.Tasks.CreateComment;

public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, ResultResponse<CreateCommentResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<CreateCommentHandler> _logger;

    public CreateCommentHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<CreateCommentHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<CreateCommentResult>> Handle(CreateCommentCommand command, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {CreateCommentHandler} triggered to handle {CreateCommentCommand}",
            nameof(CreateCommentHandler), command);

        var task = await _eclipseUnitOfWork.TaskRepository.GetByIdIncludeAsync(command.TaskId, query => query.Include(
            x => x.Histories), cancellationToken);

        if (task is null)
        {
            _logger.LogError("Task with id {TaskId} not found.", command.TaskId);
            return ResultResponse<CreateCommentResult>.FailureResult($"Task with id {command.TaskId} not found");
        }

        var taskHistory = TaskHistory.CreteComment(
            taskId: command.TaskId,
            userId: command.UserId,
            content: command.Content);

        task.AddHistory(taskHistory);

        await _eclipseUnitOfWork.TaskRepository.UpdateAsync(task, cancellationToken);
        await _eclipseUnitOfWork.SaveChangesAsync(cancellationToken);

        return ResultResponse<CreateCommentResult>.SuccessResult(CreateCommentResult.Create(taskHistory.Id));
    }
}