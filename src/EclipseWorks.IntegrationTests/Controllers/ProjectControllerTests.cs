using System.Net;
using System.Net.Http.Json;
using EclipseWorks.API.Controllers;
using EclipseWorks.API.Requests.Projects;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
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
        var result = await response.Content.ReadFromJsonAsync<CreateProjectResult>();
        result.Should().NotBeNull();
        result!.Name.Should().Be(request.Name);
        result.Description.Should().Be(request.Description);
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
}