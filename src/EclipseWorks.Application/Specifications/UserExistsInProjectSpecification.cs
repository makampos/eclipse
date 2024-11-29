using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Specifications;

public class UserExistsInProjectSpecification : ISpecification<Project>
{
    private readonly int _userId;
    private readonly int _projectId;

    public UserExistsInProjectSpecification(int userId, int projectId)
    {
        _userId = userId;
        _projectId = projectId;
    }

    public bool IsSatisfiedBy(Project project)
    {
        return project.ProjectUsers.Any(pu => pu.UserId == _userId && pu.ProjectId == _projectId);
    }
}