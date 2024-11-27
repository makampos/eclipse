using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;

namespace EclipseWorks.UnitTests.Features.Handlers;

public class CreateTaskHandlerTests : BaseTestHandler<CreateTaskHandler>
{
    private readonly CreateTaskHandler _handler;

    public CreateTaskHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[] {EclipseUnitOfWork, _logger});
    }

    [Fact(DisplayName = $"Given a valid command, When {nameof(CreateTaskHandler)} is called, Then a new task is created")]
    public async Task GivenAValidCommand_WhenCreateTaskHandlerIsCalled_ThenANewTaskIsCreated()
    {
        // Setup
        var projectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var project = projectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project, CancellationToken.None);
        await EclipseUnitOfWork.SaveChangesAsync();

        // Given
        var command = CreateTaskHandlerFaker.GenerateValidCommand(project.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data!.Name.Should().Be(command.Name);
        result.Data.Description.Should().Be(command.Description);
        result.Data.ProjectId.Should().Be(project.Id);
    }

    [Fact(DisplayName =
        $"Given a valid command, When {nameof(CreateTaskHandler)} is called, And attempt to create a task " +
        $"when exceeding the maximum number of tasks in a project, Then a failure result is returned")]
    public async Task
        GivenAValidCommand_WhenCreateTaskHandlerIsCalled_AndAttemptToCreateTaskExceedingMaxTasks_ThenFailureResultIsReturned()
    {
        // Setup
        var projectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var project = projectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project, CancellationToken.None);
        await EclipseUnitOfWork.SaveChangesAsync();

        var tasksCommands = CreateTaskHandlerFaker.GenerateValidCommands(20, project.Id);
        foreach (var taskCommand in tasksCommands)
        {
            await _handler.Handle(taskCommand, CancellationToken.None);
        }

        // Given
        var command = CreateTaskHandlerFaker.GenerateValidCommand(project.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().Be($"Project with id {command.ProjectId} has reached the maximum number of tasks");
    }

    [Fact(DisplayName =
        $"Given a valid command, When {nameof(CreateTaskHandler)} is called, And attempt to create a task " +
        $"for a project that does not exist, Then a failure result is returned")]
    public async Task GivenAValidCommand_WhenCreateTaskHandlerIsCalled_AndAttemptToCreateTaskForNonExistentProject_ThenFailureResultIsReturned()
    {
        // Given
        var command = CreateTaskHandlerFaker.GenerateValidCommand(0);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().Be($"Project with id {command.ProjectId} not found");
    }
}