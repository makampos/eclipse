using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using MediatR;

namespace EclipseWorks.Application.Features.PerformanceReport;

public record GetAverageCompletedTasksPerUserCommand(int UserId, DateOnly StartDate, DateOnly EndDate)
    : IRequest<ResultResponse<PerformanceReportResult>>
{
    public GetAverageCompletedTasksPerUserCommand() : this(0, DateOnly.MinValue, DateOnly.MinValue) { }

    public static GetAverageCompletedTasksPerUserCommand Create(int userId, DateOnly startDate, DateOnly endDate) =>
        new GetAverageCompletedTasksPerUserCommand(userId, startDate, endDate);
}