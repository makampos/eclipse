using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Tasks.UpdateTask;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;

namespace EclipseWorks.UnitTests.Features.Handlers;

public class UpdateTaskHandlerTests : BaseTestHandler<UpdateTaskHandler>
{
    private readonly UpdateTaskHandler _handler;

    public UpdateTaskHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[]{ EclipseUnitOfWork, _logger} );
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a task is not found and an error " +
                        "message is returned")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenATaskIsNotFoundAndErrorMessageIsReturned()
    {
        // Given
        var command = UpdateTaskHandlerFaker.GenerateValidCommand(1);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Data.Should().BeNull();
        result.ErrorMessage.Should().Be($"Task with id {command.Id} not found");
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a task is updated")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenATaskIsUpdated()
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
        var command = UpdateTaskHandlerFaker.GenerateValidCommand(task.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data.Should().NotBeNull();
        result.Data!.Message.Should().Be("Task updated successfully");
    }
}