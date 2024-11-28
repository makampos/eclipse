using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;

namespace EclipseWorks.UnitTests.Features.Handlers;

public class GetProjectHandlerTests : BaseTestHandler<GetProjectHandler>
{
    private readonly GetProjectHandler _handler;

    public GetProjectHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[] { EclipseUnitOfWork, _logger });
    }

    [Fact(DisplayName = "Given valid request, When HandleAsync is called, Then return project")]
    public async Task GivenValidRequest_WhenHandleAsyncIsCalled_ThenReturnProject()
    {
        // Setup
        var createProjectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var project = createProjectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project);
        await EclipseUnitOfWork.SaveChangesAsync();
        var projectId = project.Id;
        var createTaskCommand = CreateTaskHandlerFaker.GenerateValidCommand(projectId);
        var task = createTaskCommand.MapToEntity();
        await EclipseUnitOfWork.TaskRepository.AddAsync(task);
        await EclipseUnitOfWork.SaveChangesAsync();

        // Given
        var command = GetProjectCommand.Create(project.Id);

        // When
        var response = await _handler.Handle(command, CancellationToken.None);

        // Then
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Id.Should().Be(project.Id);
        response.Data!.Name.Should().Be(project.Name);
        response.Data!.Description.Should().Be(project.Description);
        response.Data!.Tasks.Should().NotBeNullOrEmpty();
        response.Data!.Tasks.Should().HaveCount(1);
        response.Data!.Tasks.First().Id.Should().Be(task.Id);
    }

    [Fact(DisplayName = "Given a valid request, When HandleAsync is called And project has deleted task, Then return project without deleted task")]
    public async Task GivenValidRequest_WhenHandleAsyncIsCalledAndProjectHasDeletedTask_ThenReturnProjectWithoutDeletedTask()
    {
        // Setup
        var createProjectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var project = createProjectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project);
        await EclipseUnitOfWork.SaveChangesAsync();
        var projectId = project.Id;
        var createTaskCommand = CreateTaskHandlerFaker.GenerateValidCommand(projectId);
        var task = createTaskCommand.MapToEntity();
        await EclipseUnitOfWork.TaskRepository.AddAsync(task);
        await EclipseUnitOfWork.SaveChangesAsync();
        await EclipseUnitOfWork.TaskRepository.DeleteAsync(task);
        await EclipseUnitOfWork.SaveChangesAsync();

        // Given
        var command = GetProjectCommand.Create(project.Id);

        // When
        var response = await _handler.Handle(command, CancellationToken.None);

        // Then
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Id.Should().Be(project.Id);
        response.Data!.Name.Should().Be(project.Name);
        response.Data!.Description.Should().Be(project.Description);
        response.Data!.Tasks.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given a valid request, When HandleAsync is called And project not found, Then return failure result")]
    public async Task GivenValidRequest_WhenHandleAsyncIsCalledAndProjectNotFound_ThenReturnFailureResult()
    {
        // Given
        var command = GetProjectCommand.Create(0);

        // When
        var response = await _handler.Handle(command, CancellationToken.None);

        // Then
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be($"Project with id: {command.Id} not found");
    }
}