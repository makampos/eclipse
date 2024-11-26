using EclipseWorks.Application.Features.Handlers;

namespace EclipseWorks.UnitTests.Handlers;

[Collection("Unit Tests")]
public class SampleCommandHandlerTests : BaseTestHandler<SampleHandler>
{
    private readonly SampleHandler _handler;
    public SampleCommandHandlerTests()
    {
        _handler = CreateHandler(parameters: new object[]{ SampleUnitOfWork, _logger} );
    }
}