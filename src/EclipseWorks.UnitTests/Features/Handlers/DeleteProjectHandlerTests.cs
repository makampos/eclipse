using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;

namespace EclipseWorks.UnitTests.Features.Handlers;

public class DeleteProjectHandlerTests : BaseTestHandler<DeleteProjectHandler>
{
    private readonly DeleteProjectHandler _handler;

    public DeleteProjectHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[]{ EclipseUnitOfWork, _logger} );
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a project is deleted")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenAProjectIsDeleted()
    {
        // Setup
        var createProjectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var project = createProjectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project);
        await EclipseUnitOfWork.SaveChangesAsync();

        // Given
        var command = DeleteProjectHandlerFaker.GenerateValidCommand(project.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data.Should().NotBeNull();
        result.Data!.Message.Should().Be($"Project with name: {project.Name} has been deleted successfully");

    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a project is not found and an error message is returned")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenAProjectIsNotFoundAndErrorMessageIsReturned()
    {
        // Given
        var command = DeleteProjectHandlerFaker.GenerateValidCommand(1);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Data.Should().BeNull();
        result.ErrorMessage.Should().Be($"Project with id: {command.Id} not found");
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a project has incomplete tasks and an error message is returned")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenAProjectHasIncompleteTasksAndErrorMessageIsReturned()
    {
        // Setup
        var createProjectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var project = createProjectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project);
        await EclipseUnitOfWork.SaveChangesAsync();

        var createTaskCommand = CreateTaskHandlerFaker.GenerateValidCommand(project.Id);
        await EclipseUnitOfWork.TaskRepository.AddAsync(createTaskCommand.MapToEntity());
        await EclipseUnitOfWork.SaveChangesAsync();

        // Given
        var command = DeleteProjectHandlerFaker.GenerateValidCommand(project.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Data.Should().BeNull();
        result.ErrorMessage.Should().Be("Project has incomplete tasks, please complete all tasks or delete them first");
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a project is soft deleted and tasks are soft deleted")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenAProjectIsSoftDeletedAndTasksAreSoftDeleted()
    {
        // Setup
        var createProjectCommand = CreateProjectHandlerFaker.GenerateValidCommand();
        var project = createProjectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project);
        await EclipseUnitOfWork.SaveChangesAsync();

        var createTaskCommand = CreateTaskHandlerFaker.GenerateValidCommand(project.Id);
        var task = createTaskCommand.MapToEntity();
        task.MarkAsCompleted();
        await EclipseUnitOfWork.TaskRepository.AddAsync(task);
        await EclipseUnitOfWork.SaveChangesAsync();

        // Given
        var command = DeleteProjectHandlerFaker.GenerateValidCommand(project.Id);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data.Should().NotBeNull();
        result.Data!.Message.Should().Be($"Project with name: {project.Name} has been deleted successfully");

        var projectFromDb = await EclipseUnitOfWork.ProjectRepository.GetByIdAsync(project.Id);
        projectFromDb.Should().BeNull();

        var taskFromDb = await EclipseUnitOfWork.TaskRepository.GetByIdAsync(task.Id);
        taskFromDb.Should().BeNull();
    }
}