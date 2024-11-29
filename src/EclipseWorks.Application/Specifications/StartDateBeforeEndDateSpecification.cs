using EclipseWorks.Application.Features.PerformanceReport;
using EclipseWorks.Domain.Interfaces.Abstractions;

namespace EclipseWorks.Application.Specifications;

public class StartDateBeforeEndDateSpecification : ISpecification<GetAverageCompletedTasksPerUserCommand>
{
    public bool IsSatisfiedBy(GetAverageCompletedTasksPerUserCommand command)
    {
        return command.StartDate > command.EndDate;
    }
}