using System.Net;
using System.Net.Http.Json;
using EclipseWorks.API.Controllers;
using EclipseWorks.API.Requests.Tasks;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Users.CreateUser;
using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Results;
using EclipseWorks.IntegrationTests.Factories;
using EclipseWorks.IntegrationTests.TestData;
using FluentAssertions;

namespace EclipseWorks.IntegrationTests.Controllers;

public class ProjectControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ProjectControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.InitializeAsync().GetAwaiter().GetResult();
        _client = _factory.CreateClient();
    }

    [Fact(DisplayName = $"Given a valid request, When {nameof(ProjectController)} is called, Then a project is created")]
    public async Task GivenAValidRequest_WhenProjectsControllerIsCalled_ThenAProjectIsCreated()
    {
        // Setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        // Given
        var request = CreateProjectRequestFaker.GenerateValidRequest(userId);

        // When
        var response = await _client.PostAsJsonAsync("/api/projects", request);
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        result.Should().NotBeNull();
        result!.Data!.Name.Should().Be(request.Name);
        result.Data!.Description.Should().Be(request.Description);
    }

    [Fact(DisplayName = $"Given a valid request, When {nameof(ProjectController)} is called, Then a project is deleted")]
    public async Task GivenAValidRequest_WhenProjectsControllerIsCalled_ThenAProjectIsDeleted()
    {
        // Setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var project = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;
        var taskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var createTaskResponse = await _client.PostAsJsonAsync("/api/tasks", taskRequest);
        createTaskResponse.EnsureSuccessStatusCode();
        var taskResult = await createTaskResponse.Content.ReadFromJsonAsync<ResultResponse<CreateTaskResult>>();
        var taskId = taskResult!.Data!.Id;
        var updateTaskRequest = UpdateTaskStatusRequest.Create(taskId, Status.Completed, userId);
        var completeTaskResponse = await _client.PatchAsJsonAsync($"/api/tasks/{taskId}/status", updateTaskRequest);
        completeTaskResponse.EnsureSuccessStatusCode();

        // Give
        // When
        var response = await _client.DeleteAsync($"/api/projects/{projectId}");
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ResultResponse<DeleteProjectResult>>();
        result.Should().NotBeNull();
        result!.Data!.Message.Should().Be($"Project with name: {project.Name} has been deleted successfully");
    }

    [Fact(DisplayName = "Given a project does not exist, When DeleteProjectAsync is called," +
                        " Then a 400 status code is returned")]
    public async Task GivenAProjectDoesNotExist_WhenDeleteProjectAsyncIsCalled_ThenA400StatusCodeIsReturned()
    {
        // Given
        var projectId = 0;

        // When
        var response = await _client.DeleteAsync($"/api/projects/{projectId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be($"Project with id: {projectId} not found");
    }

    [Fact(DisplayName = $"Given a project has incomplete tasks, When {nameof(ProjectController)} is called, " +
                        "Then a 400 status code is returned")]
    public async Task GivenAProjectHasIncompleteTasks_WhenDeleteProjectAsyncIsCalled_ThenA400StatusCodeIsReturned()
    {
        // Setup
        var createUserCommand = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserCommand);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var project = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;
        var taskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var createTaskResponse = await _client.PostAsJsonAsync("/api/tasks", taskRequest);
        createTaskResponse.EnsureSuccessStatusCode();

        // Given
        // When
        var response = await _client.DeleteAsync($"/api/projects/{projectId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be("Project has incomplete tasks, please complete all tasks or delete them first");
    }

    [Fact(DisplayName = "Given a valid request, When GetProjectAsync is called, Then a project is returned")]
    public async Task GivenAValidRequest_WhenGetProjectAsyncIsCalled_ThenAProjectIsReturned()
    {
        // Setup
        var createUserCommand = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserCommand);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var project = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;
        var taskCommand = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var createTaskResponse = await _client.PostAsJsonAsync("/api/tasks", taskCommand);
        createTaskResponse.EnsureSuccessStatusCode();

        // Given
        // When
        var response = await _client.GetAsync($"/api/projects/{projectId}");

        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ResultResponse<GetProjectResult>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
        result!.Data!.Id.Should().Be(projectId);
        result.Data!.Description.Should().Be(project.Description);
        result.Data!.Name.Should().Be(project.Name);
        result.Data!.Tasks.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Given a project does not exist, When GetProjectAsync is called, Then a 400 status code is returned")]
    public async Task GivenAProjectDoesNotExist_WhenGetProjectAsyncIsCalled_ThenA400StatusCodeIsReturned()
    {
        // Given
        var projectId = 0;

        // When
        var response = await _client.GetAsync($"/api/projects/{projectId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be($"Project with id: {projectId} not found");
    }

    [Fact(DisplayName = "Given a valid request, When GetProjectsByUserAsync is called, Then a list of projects is returned")]
    public async Task GivenAValidRequest_WhenGetProjectsByUserAsyncIsCalled_ThenAListOfProjectsIsReturned()
    {
        // Setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var createProjectRequest = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var createResponse = await _client.PostAsJsonAsync("/api/projects", createProjectRequest);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;
        var createTaskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId, userId);
        var createTaskResponse = await _client.PostAsJsonAsync("/api/tasks", createTaskRequest);
        createTaskResponse.EnsureSuccessStatusCode();

        // Given
        // When
        var response = await _client.GetAsync($"/api/projects/user/{userId}");

        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ResultResponse<PagedResult<GetProjectResult>>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();
        result!.Data!.Items.Should().HaveCount(1);
        result.Data.PageSize.Should().Be(10);
        result.Data.CurrentPage.Should().Be(1);
        result.Data.TotalPages.Should().Be(1);
        result.Data.HasNextPage.Should().BeFalse();
        result.Data.HasPreviousPage.Should().BeFalse();
    }

    [Fact(DisplayName = "Given a valid request, When AddUserAsync is called, Then a user is added to the project")]
    public async Task GivenAValidRequest_WhenAddUserAsyncIsCalled_ThenAUserIsAddedToTheProject()
    {
        // Setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var project = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;

        // create second user
        var createUserRequest2 = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse2 = await _client.PostAsJsonAsync("/api/users", createUserRequest2);
        createUserResponse2.EnsureSuccessStatusCode();
        var createUserResult2 = await createUserResponse2.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var secondUserId = createUserResult2!.Data!.Id;

        // Given
        var request = AddUserRequestFaker.GenerateValidRequest(secondUserId, projectId);

        // When
        var response = await _client.PutAsJsonAsync($"/api/projects/{projectId}/users", request);
        response.EnsureSuccessStatusCode();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "Given a user already exists in the project, When AddUserAsync is called, Then a 400 status code is returned")]
    public async Task GivenAUserAlreadyExistsInTheProject_WhenAddUserAsyncIsCalled_ThenA400StatusCodeIsReturned()
    {
        // Setup
        var createUserRequest = CreateUserRequestFaker.GenerateValidRequest();
        var createUserResponse = await _client.PostAsJsonAsync("/api/users", createUserRequest);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResult = await createUserResponse.Content.ReadFromJsonAsync<ResultResponse<CreateUserResult>>();
        var userId = createUserResult!.Data!.Id;
        var project = CreateProjectRequestFaker.GenerateValidRequest(userId);
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;

        // Given
        var request = AddUserRequestFaker.GenerateValidRequest(userId, projectId);

        // When
        var response = await _client.PutAsJsonAsync($"/api/projects/{projectId}/users", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Be($"User with id {userId} already exists in project with id {projectId}");
    }
}