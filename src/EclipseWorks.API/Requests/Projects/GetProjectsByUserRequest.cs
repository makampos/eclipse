namespace EclipseWorks.API.Requests.Projects;

public record GetProjectsByUserRequest(int Id, int PageNumber, int PageSize)
{
    public static GetProjectsByUserRequest Create(int id, int pageNumber, int pageSize) => new(id, pageNumber, pageSize);
}