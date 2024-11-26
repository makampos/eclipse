using EclipseWorks.IntegrationTests.Factories;

namespace EclipseWorks.IntegrationTests.Controllers;

[Collection("Integration Tests")]
public class SampleControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public SampleControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.InitializeAsync().GetAwaiter().GetResult();
        _client = _factory.CreateClient();
    }
}