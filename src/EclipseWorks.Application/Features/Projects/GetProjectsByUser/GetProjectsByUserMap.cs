using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Features.GetProjectsByUser;

public static class GetProjectsByUserMap
{
    public static List<GetProjectResult> MapToGetProjectsResult(IReadOnlyCollection<Project> projects)
    {
        if (!projects.Any())
        {
            return [];
        }

        return projects.Select(p => GetProjectResult.Create(p.Id, p.Name, p.Description, p.Tasks)).ToList();
    }
}