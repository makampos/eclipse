using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.GetProjectsByUser;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Application.Features.Tasks.DeleteTask;
using EclipseWorks.Application.Features.Users.CreateUser;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.UnitTests.Features.Handlers;

public class GetProjectsByUserHandlerTests: BaseTestHandler<GetProjectsByUserHandler>
{
    private readonly GetProjectsByUserHandler _handler;

    public GetProjectsByUserHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[] { EclipseUnitOfWork, _logger });
    }

    [Fact(DisplayName = "Given a valid command, When GetProjectsByUserHandler is called, Then it should return a list of projects")]
    public async Task GivenAValidCommand_WhenGetProjectsByUserHandlerIsCalled_ThenItShouldReturnAListOfProjects()
    {
        // Setup
        var createUserCommand = CreateUserHandlerFaker.GenerateValidCommand();
        var user = createUserCommand.MapToEntity();
        await EclipseUnitOfWork.UserRepository.AddAsync(user);
        await EclipseUnitOfWork.SaveChangesAsync();
        var createProjectCommands = CreateProjectHandlerFaker.GenerateValidCommands(user.Id, 10);
        var createProjectHandler = new CreateProjectHandler(EclipseUnitOfWork, new Logger<CreateProjectHandler>(new LoggerFactory()));
        var projectIds = new List<int>();
        var taskIds = new List<int>();
        foreach (var createProjectCommand in createProjectCommands)
        {
            var createProjectHandlerResult = await createProjectHandler.Handle(createProjectCommand, CancellationToken.None);
            createProjectHandlerResult.Success.Should().BeTrue();
            var projectId = createProjectHandlerResult.Data!.Id;
            projectIds.Add(projectId);

            var createTaskHandler = new CreateTaskHandler(EclipseUnitOfWork, new Logger<CreateTaskHandler>(new LoggerFactory()));

            var createTaskCommand = CreateTaskHandlerFaker.GenerateValidCommand(projectId, user.Id);
            var createTaskHandlerResult = await createTaskHandler.Handle(createTaskCommand, CancellationToken.None);
            createTaskHandlerResult.Success.Should().BeTrue();
            var taskId = createTaskHandlerResult.Data!.Id;
            taskIds.Add(taskId);
        }


        // Given
        var getProjectsByUserCommand = GetProjectsByUserHandlerFaker.GenerateValidCommand(user.Id, 1, 10);

        // delete task
        var taskIdToDelete = taskIds.First();
        var deleteTaskCommand = DeleteTaskCommand.Create(taskIdToDelete);
        var deleteTaskHandler = new DeleteTaskHandler(EclipseUnitOfWork, new Logger<DeleteTaskHandler>(new LoggerFactory()));
        var deleteTaskHandlerResult = await deleteTaskHandler.Handle(deleteTaskCommand, CancellationToken.None);
        deleteTaskHandlerResult.Success.Should().BeTrue();

        // When
        var result = await _handler.Handle(getProjectsByUserCommand, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.PageSize.Should().Be(10);
        result.Data.CurrentPage.Should().Be(1);
        result.Data.TotalCount.Should().Be(10);
        result.Data.Items.Should().NotBeNullOrEmpty();
        result.Data.Items.Select(t => t.Tasks.Any(r => r.Id == taskIdToDelete).Should().BeFalse());
    }
}