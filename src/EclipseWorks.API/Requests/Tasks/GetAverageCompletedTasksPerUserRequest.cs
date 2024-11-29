namespace EclipseWorks.API.Requests.Tasks;

public record GetAverageCompletedTasksPerUserRequest(int UserId, DateOnly StartDate, DateOnly EndDate)
{
    public GetAverageCompletedTasksPerUserRequest() : this(0, DateOnly.MinValue, DateOnly.MinValue) { }

    public static GetAverageCompletedTasksPerUserRequest Create(int userId, DateOnly startDate, DateOnly endDate) =>
        new GetAverageCompletedTasksPerUserRequest(userId, startDate, endDate);
}