using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Tasks.UpdateTask;
using EclipseWorks.Application.Features.Users.CreateUser;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.UnitTests.Features.Handlers;

[Collection("Unit Tests")]
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
        var createUserCommand = CreateUserHandlerFaker.GenerateValidCommand();
        var user = createUserCommand.MapToEntity();
        await EclipseUnitOfWork.UserRepository.AddAsync(user);
        await EclipseUnitOfWork.SaveChangesAsync();
        var userId = user.Id;
        var createProjectCommand = CreateProjectHandlerFaker.GenerateValidCommand(userId);
        var project = createProjectCommand.MapToEntity();
        await EclipseUnitOfWork.ProjectRepository.AddAsync(project);
        await EclipseUnitOfWork.SaveChangesAsync();
        var projectId = project.Id;
        var createTaskCommand = CreateTaskHandlerFaker.GenerateValidCommand(projectId, userId);

        var createTaskHandler =
            new CreateTaskHandler(EclipseUnitOfWork, new Logger<CreateTaskHandler>(new LoggerFactory()));
        var createTaskHandlerResult = await createTaskHandler.Handle(createTaskCommand, CancellationToken.None);
        createTaskHandlerResult.Success.Should().BeTrue();
        var taskId = createTaskHandlerResult.Data!.Id;

        // Given
        var command = UpdateTaskHandlerFaker.GenerateValidCommand(taskId);

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