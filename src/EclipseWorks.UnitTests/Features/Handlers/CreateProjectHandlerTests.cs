using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.UnitTests.Features.TestData;
using FluentAssertions;

namespace EclipseWorks.UnitTests.Features.Handlers;

[Collection("Unit Tests")]
public class CreateProjectHandlerTests : BaseTestHandler<CreateProjectHandler>
{
    private readonly CreateProjectHandler _handler;
    public CreateProjectHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[]{ EclipseUnitOfWork, _logger} );
    }

    [Fact(DisplayName = "Given a valid command, When Handler is called, Then a project is created")]
    public async Task GivenAValidCommand_WhenHandlerIsCalled_ThenAProjectIsCreated()
    {
        // Given
        var command = CreateProjectHandlerFaker.GenerateValidCommand();

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Data!.Name.Should().Be(command.Name);
        result.Data.Description.Should().Be(command.Description);
    }
}