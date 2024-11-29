using EclipseWorks.Application.Features.AddUser;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.Users.CreateUser;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.UnitTests.Features.Handlers;

public class AddUserHandlerTests : BaseTestHandler<AddUserHandler>
{
    private readonly AddUserHandler _handler;
    private readonly CreateUserHandler _createUserHandler;
    private readonly CreateProjectHandler _createProjectHandler;

    public AddUserHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[]{ EclipseUnitOfWork, _logger} );
        _createUserHandler = new CreateUserHandler(EclipseUnitOfWork, new Logger<CreateUserHandler>(new LoggerFactory()));
        _createProjectHandler = new CreateProjectHandler(EclipseUnitOfWork, new Logger<CreateProjectHandler>(new LoggerFactory()));
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a user is added to the project")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenAUserIsAddedToTheProject()
    {
        // setup
        var userCommand = CreateUserHandlerFaker.GenerateValidCommand();
        var createUserHandlerResult = await _createUserHandler.Handle(userCommand, CancellationToken.None);
        createUserHandlerResult.Success.Should().BeTrue();
        var userId = createUserHandlerResult.Data!.Id;

        var projectCommand = CreateProjectHandlerFaker.GenerateValidCommand(userId);
        var createProjectHandlerResult = await _createProjectHandler.Handle(projectCommand, CancellationToken.None);
        createProjectHandlerResult.Success.Should().BeTrue();
        var projectId = createProjectHandlerResult.Data!.Id;

        // Create a new user to add to the project
        var secondCreateUserCommand = CreateUserHandlerFaker.GenerateValidCommand();
        var secondCreateUserResult = await _createUserHandler.Handle(secondCreateUserCommand, CancellationToken.None);
        secondCreateUserResult.Success.Should().BeTrue();
        var secondUserId = secondCreateUserResult.Data!.Id;

        // Given
        var command = AddUserHandlerFaker.GenerateValidCommand(projectId: projectId, userId: secondUserId);

        // When
        var addUserHandlerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        addUserHandlerResult.Should().NotBeNull();
        addUserHandlerResult.Success.Should().BeTrue();
        addUserHandlerResult.Data.Should().NotBeNull();
        addUserHandlerResult.ErrorMessage.Should().BeNullOrEmpty();
        addUserHandlerResult.Data!.Message.Should().Be("User added to project successfully");
    }

    [Fact(DisplayName = "GIven a valid command, When user already exists in the project, Then return a failure result")]
    public async Task GivenAValidCommand_WhenUserAlreadyExistsInTheProject_ThenReturnAFailureResult()
    {
        // setup
        var userCommand = CreateUserHandlerFaker.GenerateValidCommand();
        var createUserHandlerResult = await _createUserHandler.Handle(userCommand, CancellationToken.None);
        createUserHandlerResult.Success.Should().BeTrue();
        var userId = createUserHandlerResult.Data!.Id;

        var projectCommand = CreateProjectHandlerFaker.GenerateValidCommand(userId);
        var createProjectHandlerResult = await _createProjectHandler.Handle(projectCommand, CancellationToken.None);
        createProjectHandlerResult.Success.Should().BeTrue();
        var projectId = createProjectHandlerResult.Data!.Id;

        // Given
        var command = AddUserHandlerFaker.GenerateValidCommand(projectId: projectId, userId: userId);

        // When
        var addUserHandlerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        addUserHandlerResult.Should().NotBeNull();
        addUserHandlerResult.Success.Should().BeFalse();
        addUserHandlerResult.Data.Should().BeNull();
        addUserHandlerResult.ErrorMessage.Should().Be($"User with id {userId} already exists in project with id {projectId}");
    }

    [Fact(DisplayName = "Given a valid command, When project does not exist, Then return a failure result")]
    public async Task GivenAValidCommand_WhenProjectDoesNotExist_ThenReturnAFailureResult()
    {
        // setup
        var userCommand = CreateUserHandlerFaker.GenerateValidCommand();
        var createUserHandlerResult = await _createUserHandler.Handle(userCommand, CancellationToken.None);
        createUserHandlerResult.Success.Should().BeTrue();
        var userId = createUserHandlerResult.Data!.Id;

        // Given
        var command = AddUserHandlerFaker.GenerateValidCommand(projectId: 0, userId: userId);

        // When
        var addUserHandlerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        addUserHandlerResult.Should().NotBeNull();
        addUserHandlerResult.Success.Should().BeFalse();
        addUserHandlerResult.Data.Should().BeNull();
        addUserHandlerResult.ErrorMessage.Should().Be("Project with id 0 not found");
    }

    // create a test for multiples users added to the project
    [Fact(DisplayName =
        "Given a valid command, When multiple users are added to the project, Then return a success result")]
    public async Task GivenAValidCommand_WhenMultipleUsersAreAddedToTheProject_ThenReturnASuccessResult()
    {
        // setup
        var userCommand = CreateUserHandlerFaker.GenerateValidCommand();
        var createUserHandlerResult = await _createUserHandler.Handle(userCommand, CancellationToken.None);
        createUserHandlerResult.Success.Should().BeTrue();
        var userId = createUserHandlerResult.Data!.Id;

        var projectCommand = CreateProjectHandlerFaker.GenerateValidCommand(userId);
        var createProjectHandlerResult = await _createProjectHandler.Handle(projectCommand, CancellationToken.None);
        createProjectHandlerResult.Success.Should().BeTrue();
        var projectId = createProjectHandlerResult.Data!.Id;

        // Create a new user to add to the project
        var createUserCommands = CreateUserHandlerFaker.GenerateValidCommands(10);
        var userIds = new List<int>();
        foreach (var createUserCommand in createUserCommands)
        {
            var resultResponse = await _createUserHandler.Handle(createUserCommand, CancellationToken.None);
            resultResponse.Success.Should().BeTrue();
            userIds.Add(resultResponse.Data!.Id);
        }

        // Given
        // When
        foreach (var id in userIds)
        {
            var command = AddUserHandlerFaker.GenerateValidCommand(projectId: projectId, userId: id);
            var addUserHandlerResult = await _handler.Handle(command, CancellationToken.None);
            addUserHandlerResult.Success.Should().BeTrue();
        }

        // Then
        var project = await EclipseUnitOfWork.ProjectRepository.GetByIdIncludeAsync(
            projectId, query => query.Include(p => p.ProjectUsers));

        project.Should().NotBeNull();

        project.ProjectUsers.Count.Should().Be(11, "because 1 user was added when the project was created and 10 users were added after");

    }

}