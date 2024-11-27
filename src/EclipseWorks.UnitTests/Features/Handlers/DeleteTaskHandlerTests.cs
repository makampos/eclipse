using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Tasks.DeleteTask;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EclipseWorks.UnitTests.Features.Handlers;

public class DeleteTaskHandlerTests : BaseTestHandler<DeleteTaskHandler>
{
    private readonly DeleteTaskHandler _handler;

    public DeleteTaskHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[] { EclipseUnitOfWork, _logger });
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a task is soft deleted")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenATaskIsSoftDeleted()
    {
        // Setup
        var projectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var projectEntity = projectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(projectEntity);
        await EclipseUnitOfWork.SaveChangesAsync();
        var projectId = projectEntity.Id;
        var createTaskCommand = CreateTaskHandlerFaker.GenerateValidCommand(projectId);
        var task = createTaskCommand.MapToEntity();
        await EclipseUnitOfWork.TaskRepository.AddAsync(task);
        await EclipseUnitOfWork.SaveChangesAsync();

        // Given
        var command = DeleteTaskCommand.Create(task.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data.Should().NotBeNull();
        result.Data!.Message.Should().Be($"Task with name: {task.Name} has been deleted successfully");

        var project = await EclipseUnitOfWork.ProjectRepository.GetByIdIncludeAsync(
            projectId, query => query.Include(t => t.Tasks),
            CancellationToken.None);

        project!.Tasks.Should().Contain(t => t.Id == command.Id && t.IsDeleted == true,
            "Task should be soft deleted");
    }

    [Fact(DisplayName =
        "Given a valid command, When Handler is called, Then a task is not found and an error message is returned")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenATaskIsNotFoundAndAnErrorMessageIsReturned()
    {
        // Given
        var command = DeleteTaskCommand.Create(1);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Task with id: {command.Id} not found");
        result.Data.Should().BeNull();
    }
}