using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Specifications;

public class MaxTasksSpecification : ISpecification<Project>
{
    private readonly int _maxTasks;

    public MaxTasksSpecification(int maxTasks)
    {
        _maxTasks = maxTasks;
    }

    public bool IsSatisfiedBy(Project project)
    {
        return project.Tasks.Count >= _maxTasks;
    }
}