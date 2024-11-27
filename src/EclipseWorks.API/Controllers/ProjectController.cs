using EclipseWorks.API.Requests.Projects;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(IMediator mediator, ILogger<ProjectController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType<ResultResponse<CreateProjectResult>>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectRequest request)
    {
        _logger.LogInformation("Controller {ProjectController} triggered to handle {CreateProjectRequest}",
            nameof(ProjectController), request);
        var command = request.ToCreateProjectCommand();
        var result = await _mediator.Send(command);
        return CreatedAtRoute(string.Empty, new { id = result.Data!.Id }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType<ResultResponse<DeleteProjectResult>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProjectAsync([FromRoute] int id)
    {
        _logger.LogInformation("Controller {ProjectController} triggered to handle {DeleteProjectRequest}",
            nameof(ProjectController), id);
        var request = DeleteProjectRequest.Create(id);
        var command = request.ToDeleteProjectCommand();
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            // TODO: test this scenario
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result);
    }
}