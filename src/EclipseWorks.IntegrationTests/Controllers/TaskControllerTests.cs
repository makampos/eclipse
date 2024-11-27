using System.Net;
using System.Net.Http.Json;
using EclipseWorks.API.Controllers;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Domain.Results;
using EclipseWorks.IntegrationTests.Factories;
using EclipseWorks.IntegrationTests.TestData;
using FluentAssertions;

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
        var projectCommand = CreateProjectRequestFaker.GenerateValidRequest();
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectCommand);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        // Given
        var request = CreateTaskRequestFaker.GenerateValidRequest(projectId);

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
        var projectRequest = CreateProjectRequestFaker.GenerateValidRequest();
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectRequest);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var createTaskRequests = CreateTaskRequestFaker.GenerateValidRequests(20, projectId);
        foreach (var createTasksRequests in createTaskRequests)
        {
            var createTasksResponses = await _client.PostAsJsonAsync("/api/tasks", createTasksRequests);
            createTasksResponses.EnsureSuccessStatusCode();
        }

        // Given
        var createTaskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId);

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
        var request = CreateTaskRequestFaker.GenerateValidRequest(0);

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
        var projectCommand = CreateProjectRequestFaker.GenerateValidRequest();
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectCommand);
        projectResponse.EnsureSuccessStatusCode();
        var projectId = await projectResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>()
            .ContinueWith(p => p.Result!.Data!.Id);

        var taskCommand = CreateTaskRequestFaker.GenerateValidRequest(projectId);
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
}