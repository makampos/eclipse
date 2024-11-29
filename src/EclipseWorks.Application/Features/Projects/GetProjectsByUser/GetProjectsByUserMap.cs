using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Application.Features.Tasks.GetTaskResult;
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

        return projects.Select(p => GetProjectResult.Create(p.Id, p.Name, p.Description, p.Tasks.Select(x =>
            GetTaskResult.Create(x.Id, x.Name, x.Description,x.PriorityLevel,x.Status,x.DueDate,x.CompletionDate,x.ProjectId))
            .ToList()))
            .ToList();
    }
}