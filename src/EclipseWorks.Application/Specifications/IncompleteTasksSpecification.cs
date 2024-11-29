using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Specifications;

public class IncompleteTasksSpecification : ISpecification<Project>
{
    public bool IsSatisfiedBy(Project project)
    {
        return project.Tasks.Any(x => x.Status is Status.InProgress or Status.Pending);
    }
}