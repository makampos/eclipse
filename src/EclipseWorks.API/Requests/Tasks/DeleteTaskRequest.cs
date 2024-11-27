namespace EclipseWorks.API.Requests.Tasks;

public record DeleteTaskRequest(int Id)
{
    public static DeleteTaskRequest Create(int id) => new(id);
}