using EclipseWorks.API.Requests.Projects;
using EclipseWorks.Application.Features.AddUser;
using EclipseWorks.Application.Features.CreateProject;
using EclipseWorks.Application.Features.DeleteProject;
using EclipseWorks.Application.Features.GetProject;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.API.Controllers;

/// <summary>
/// This controller handles project-related operations.
/// </summary>
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

    /// <summary>
    ///  Gets all projects for a user.
    /// </summary>
    /// <returns></returns>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType<ResultResponse<PagedResult<GetProjectResult>>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProjectsAsync([FromRoute] int userId, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Controller {ProjectController} triggered to handle {GetProjectsRequest}",
            nameof(ProjectController), userId);
        var request = GetProjectsByUserRequest.Create(userId, pageNumber, pageSize);
        var command = request.ToGetProjectsByUserCommand();
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result); //TODO: Update to use TaskResult inside ResultResponse
    }

    /// <summary>
    ///   Gets a project by its ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<ResultResponse<GetProjectResult>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProjectAsync([FromRoute] int id)
    {
        _logger.LogInformation("Controller {ProjectController} triggered to handle {GetProjectRequest}",
            nameof(ProjectController), id);
        var request = GetProjectRequest.Create(id);
        var command = request.ToGetProjectCommand();
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result); //TODO: Update to use TaskResult inside ResultResponse
    }


    /// <summary>
    /// Creates a new project.
    /// </summary>
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

    /// <summary>
    ///  Deletes a project by its ID.
    /// </summary>
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
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result);
    }

    /// <summary>
    ///  Adds a user to a project.
    /// </summary>
    [HttpPut("{projectId:int}/users")]
    [ProducesResponseType<ResultResponse<AddUserResult>>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserAsync([FromRoute] int projectId, [FromBody] AddUserRequest request)
    {
        _logger.LogInformation("Controller {ProjectController} triggered to handle {AddUserRequest}",
            nameof(ProjectController), request);
        request = request.IncludeProjectId(projectId);

        var command = request.ToAddUserCommand();
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return NoContent();
    }
}