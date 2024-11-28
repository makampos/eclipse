using System.Net;
using System.Net.Http.Json;
using EclipseWorks.API.Controllers;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
using EclipseWorks.Application.Features.GetProject;
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
        // Given
        var request = CreateProjectRequestFaker.GenerateValidRequest();

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
        var project = CreateProjectRequestFaker.GenerateValidRequest();
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;
        var taskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId);
        var createTaskResponse = await _client.PostAsJsonAsync("/api/tasks", taskRequest);
        createTaskResponse.EnsureSuccessStatusCode();

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

    // write test for when project is not found while attempting to delete
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
        var project = CreateProjectRequestFaker.GenerateValidRequest();
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;
        var taskRequest = CreateTaskRequestFaker.GenerateValidRequest(projectId);
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
        var project = CreateProjectRequestFaker.GenerateValidRequest();
        var createResponse = await _client.PostAsJsonAsync("/api/projects", project);
        createResponse.EnsureSuccessStatusCode();
        var createProjectResult = await createResponse.Content.ReadFromJsonAsync<ResultResponse<CreateProjectResult>>();
        var projectId = createProjectResult!.Data!.Id;
        var taskCommand = CreateTaskRequestFaker.GenerateValidRequest(projectId);
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

}