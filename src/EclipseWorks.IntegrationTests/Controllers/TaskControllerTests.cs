using System.Net;
using System.Net.Http.Json;
using EclipseWorks.API.Controllers;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.Tasks.CreateComment;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Users.CreateUser;
using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using EclipseWorks.IntegrationTests.Factories;
using EclipseWorks.IntegrationTests.TestData;
using FluentAssertions;
using Task = System.Threading.Tasks.Task;

namespace EclipseWorks.IntegrationTests.Controllers;

public class TaskControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public TaskControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.InitializeAsync().GetAwaiter().GetResult();
        _client = _factory.CreateClient();
    }

    [Fact(DisplayName = $"Given a valid request, When {nameof(TaskController)} is called, Then a task is created")]
    public async Task GivenAValidRequest_WhenTasksControllerIsCalled_ThenATaskIsCreated()
    {
        // Setup
        var createUserCommand = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserCommand);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var projectCommand = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectCommand);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        // Given
        var request = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);

        // When
        var response = await _client.PostAsJsonAsync("/api/tasks", request);
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<ResultResponse<CreateTaskResult>>();
        result.Should().NotBeNull();
        result!.Data!.Name.Should().Be(request.Name);
        result.Data.Description.Should().Be(request.Description);
        result.Data.ProjectId.Should().Be(request.ProjectId);
    }

    [Fact(DisplayName =
        $"Given an invalid request, When {nameof(TaskController)} is called, And attempt to create a task " +
        $"when exceeding the maximum number of tasks in a project, Then a bad request is returned")]
    public async Task
        GivenAnInvalidRequest_WhenTasksControllerIsCalled_AndAttemptToCreateTaskExceedingMaxTasks_ThenBadRequestIsReturned()
    {
        // Setup
        var createUserCommand = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserCommand);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var projectRequest = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectRequest);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var createTaskRequests = CreateTaskRequestFaker.GenerateValidRequests(
            count: 20, projectId: projectId, userId: userId, dueDate: DateOnly.FromDateTime(DateTime.Now.AddDays(10)));
        foreach (var createTasksRequests in createTaskRequests)
        {
            var createTasksResponses = await _client.PostAsJsonAsync("/api/tasks", createTasksRequests);
            createTasksResponses.EnsureSuccessStatusCode();
        }

        // Given
        var createTaskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);

        // When
        var response = await _client.PostAsJsonAsync("/api/tasks", createTaskRequest);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Then
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be($"Project with id {projectId} has reached the maximum number of tasks");
    }

    [Fact(DisplayName = "Given an invalid request, When TaskController is called, And attempt to create a task " +
                        "for a non-existent project, Then a bad request is returned")]
    public async Task GivenAnInvalidRequest_WhenTaskControllerIsCalled_AndAttemptToCreateTaskForNonExistentProject_ThenBadRequestIsReturned()
    {
        // Given
        var request = CreateTaskRequestFaker.GenerateValidRequest(0, 0);

        // When
        var response = await _client.PostAsJsonAsync("/api/tasks", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Then
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be($"Project with id {request.ProjectId} not found");
    }

    [Fact(DisplayName = "Given a valid request, When TaskController is called, And attempt to update a task, Then a task is updated")]
    public async Task GivenAValidRequest_WhenTaskControllerIsCalled_AndAttemptToUpdateTask_ThenTaskIsUpdated()
    {
        // Setup
        var createUserCommand = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserCommand);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var projectCommand = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectCommand);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var taskCommand = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var taskResponse = await _client.PostAsJsonAsync("/api/tasks", taskCommand);
        taskResponse.EnsureSuccessStatusCode();
        var taskId = await taskResponse.Content.ReadFromJsonAsync<ResultResponse<CreateTaskResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        // Given
        var request = UpdateTaskRequestFaker.GenerateValidRequest();

        // When
        var response = await _client.PutAsJsonAsync($"/api/tasks/{taskId}", request);
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Given a valid request, When TaskController is called, And attempt to update a task, Then a bad request is returned")]
    public async Task
    GivenAValidRequest_WhenTaskControllerIsCalled_AndAttemptToUpdateTaskForNonExistingTask_ThenBadRequestIsReturned()
    {
        // Given
        var request = UpdateTaskRequestFaker.GenerateValidRequest();

        // When
        var response = await _client.PutAsJsonAsync("/api/tasks/0", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Then
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be("Task with id 0 not found");
    }

    [Fact(DisplayName = "Given a valid request, When TaskController is called, And attempt to delete a task, Then a task is deleted")]
    public async Task GivenAValidRequest_WhenTaskControllerIsCalled_AndAttemptToDeleteTask_ThenTaskIsDeleted()
    {
        // Setup
        var createUserCommand = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserCommand);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var projectCommand = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectCommand);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var taskCommand = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var taskResponse = await _client.PostAsJsonAsync("/api/tasks", taskCommand);
        taskResponse.EnsureSuccessStatusCode();
        var taskId = await taskResponse.Content.ReadFromJsonAsync<ResultResponse<CreateTaskResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        // When
        var response = await _client.DeleteAsync($"/api/tasks/{taskId}");
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Given a valid request, When TaskController is called, And attempt to delete a task, Then a bad request is returned")]
    public async Task GivenAValidRequest_WhenTaskControllerIsCalled_AndAttemptToDeleteNonExistingTask_ThenBadRequestIsReturned()
    {
        // When
        var response = await _client.DeleteAsync("/api/tasks/0");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Then
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be($"Task with id: 0 not found");
    }

    [Fact(DisplayName = "Given a valid request, When TaskController is called, And attempted to update task statuses to completed, Then tasks are updated")]
    public async Task GivenAValidRequest_WhenTaskControllerIsCalled_AndAttemptToUpdateTaskStatusesToCompleted_ThenTasksAreUpdated()
    {
        // Setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var createProjectRequest = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var createTaskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var taskResponse = await _client.PostAsJsonAsync("/api/tasks", createTaskRequest);
        taskResponse.EnsureSuccessStatusCode();
        var taskId = await taskResponse.Content.ReadFromJsonAsync<ResultResponse<CreateTaskResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        // Given
        var request = UpdateTaskStatusRequestFaker.GenerateValidaRequest(taskId, userId, Status.Completed);

        // When
        var response = await _client.PatchAsJsonAsync($"/api/tasks/{taskId}/status", request);
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Given a valid request, When TaskController is called, And attempt to add a comment to a task, Then a comment is added")]
    public async Task GivenAValidRequest_WhenTaskControllerIsCalled_AndAttemptToAddCommentToTask_ThenCommentIsAdded()
    {
        // Setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var createProjectRequest = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var createTaskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var taskResponse = await _client.PostAsJsonAsync("/api/tasks", createTaskRequest);
        taskResponse.EnsureSuccessStatusCode();
        var taskId = await taskResponse.Content.ReadFromJsonAsync<ResultResponse<CreateTaskResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        // Given
        var createCommentRequest = CreateTaskCommentRequestFaker.GenerateValidRequest(taskId, userId);

        // When
        var response = await _client.PostAsJsonAsync($"/api/tasks/{taskId}/comments", createCommentRequest);
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<ResultResponse<CreateCommentResult>>();
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
    }

    [Fact(DisplayName = "Given a valid request, When TaskController is called, And attempt to get the average number " +
                        "of completed tasks per user, Then the average number of completed tasks per user is returned")]
    public async Task
        GivenAValidRequest_WhenTaskControllerIsCalled_AndAttemptToGetAverageCompletedTasksPerUser_ThenAverageCompletedTasksPerUserIsReturned()
    {
        // setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest(Role.Manager);
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();

        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;

        var createProjectRequest = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        projectResponse.EnsureSuccessStatusCode();

        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var dueDate = DateOnly.FromDateTime(DateTime.Now);

        var createTaskRequests = CreateTaskRequestFaker.GenerateValidRequests(
            count: 10, projectId: projectId, userId: userId, dueDate: dueDate );

        var taskIds = new List<int>();
        foreach (var createTaskRequest in createTaskRequests)
        {
            var taskResponse = await _client.PostAsJsonAsync("/api/tasks", createTaskRequest);
            taskResponse.EnsureSuccessStatusCode();
            var taskId = await taskResponse.Content.ReadFromJsonAsync<ResultResponse<CreateTaskResult>>()
                .ContinueWith(p => p.Result!.Data!.Id);

            taskIds.Add(taskId);

        }

        var completedTasks = taskIds.Take(5).Select(t => t).ToList();

        foreach (var completedTask in completedTasks)
        {
            var request = UpdateTaskStatusRequestFaker.GenerateValidaRequest(completedTask, userId, Status.Completed);
            var response = await _client.PatchAsJsonAsync($"/api/tasks/{completedTask}/status", request);
            response.EnsureSuccessStatusCode();
        }

        // Given
        var startDate = DateTime.Now.AddDays(-15);
        var endDate = DateTime.Now;

        // When
        var reportResponse = await _client.GetAsync($"/api/tasks/performance-report?userId={userId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Then

        reportResponse.EnsureSuccessStatusCode();
        var result = await reportResponse.Content.ReadFromJsonAsync<ResultResponse<PerformanceReportResult>>();
        result.Should().NotBeNull();
        result!.Data!.TasksCompleted.Should().Be(5);
    }


    [Fact(DisplayName = " Given an invalid request, When TaskController is called, And attempt to get the average number " +
                        "of completed tasks per user, Then a bad request is returned")]
    public async Task GivenAnInvalidRequest_WhenTaskControllerIsCalled_AndAttemptToGetAverageCompletedTasksPerUser_ThenBadRequestIsReturned()
    {
        // Given
        var startDate = DateTime.Now.AddDays(15);
        var endDate = DateTime.Now;
        var id = 0;

        // When
        var reportResponse = await _client.GetAsync
            ($"/api/tasks/performance-report?userId={id}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Then
        reportResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await reportResponse.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be($"User with id: {id} not found");
    }

    [Fact(DisplayName = "Given an invalid request, When TaskController is called, And attempt to get the average number " +
                        "of completed tasks per user, Then a bad request is returned When user does not have the role of manager")]
    public async Task GivenAnInvalidRequest_WhenTaskControllerIsCalled_AndAttemptToGetAverageCompletedTasksPerUser_ThenBadRequestIsReturnedWhenUserIsNotManager()
    {
        // Given
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest(Role.Undefined);
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();

        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;

        var startDate = DateTime.Now.AddDays(15);
        var endDate = DateTime.Now;

        // When
        var reportResponse = await _client.GetAsync($"/api/tasks/performance-report?userId={userId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Then
        reportResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await reportResponse.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be("Current role are not allowed for this operation");
    }

    [Fact(DisplayName =
        "Given an invalid request, When TaskController is called, And attempt to get the average number " +
        "of completed tasks per user, Then a bad request is returned When start date is after end date")]
    public async Task
        GivenAnInvalidRequest_WhenTaskControllerIsCalled_AndAttemptToGetAverageCompletedTasksPerUser_ThenBadRequestIsReturnedWhenStartDateIsAfterEndDate()
    {
        // Given
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest(Role.Manager);
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();

        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;


        // When
        var reportResponse = await _client.GetAsync($"/api/tasks/performance-report?userId={userId}&startDate=2024-01-01&endDate=2023-01-01");
        reportResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await reportResponse.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be("Start date must be before end date");
    }
}