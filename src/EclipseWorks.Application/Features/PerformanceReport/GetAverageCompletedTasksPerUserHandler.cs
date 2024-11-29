using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.PerformanceReport;

public class GetAverageCompletedTasksPerUserHandler : IRequestHandler<GetAverageCompletedTasksPerUserCommand, ResultResponse<PerformanceReportResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<GetAverageCompletedTasksPerUserHandler> _logger;

    public GetAverageCompletedTasksPerUserHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<GetAverageCompletedTasksPerUserHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<PerformanceReportResult>> Handle(GetAverageCompletedTasksPerUserCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetAverageCompletedTasksPerUserHandler} triggered to handle {GetAverageCompletedTasksPerUserCommand}",
            nameof(GetAverageCompletedTasksPerUserHandler), command.UserId);

        var user = await _eclipseUnitOfWork.UserRepository.GetByIdAsync(command.UserId, cancellationToken);

        if (user is null || user.Role is not Role.Manager)
        {
            var errorMessage = user is null
                ? $"User with id {command.UserId} not found"
                : "Current role are not allowed for this operation";

            _logger.LogWarning(errorMessage);
            return ResultResponse<PerformanceReportResult>.FailureResult(errorMessage);
        }

        if (command.StartDate > command.EndDate)
        {
            _logger.LogError("Start date must be before end date");
            return ResultResponse<PerformanceReportResult>.FailureResult("Start date must be before end date");
        }

        var report = await _eclipseUnitOfWork.TaskRepository.GetAverageCompletedTasksPerUserAsync(
            command.UserId,
            command.StartDate,
            command.EndDate, cancellationToken);

        return ResultResponse<PerformanceReportResult>.SuccessResult(report);
    }
}