using EclipseWorks.API.Requests.Users;
using EclipseWorks.Application.Features.Users.CreateUser;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.API.Controllers;

/// <summary>
///  This controller handles user-related operations.
/// </summary>
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(IMediator mediator, ILogger<UserController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    ///  Creates a new user.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<ResultResponse<CreateUserResult>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        _logger.LogInformation("Controller {UserController} triggered to handle {CreateUserRequest}",
            nameof(UserController), request);

        var command = request.MapToCommand();
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Error creating user: {ErrorMessage}", result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }

        return CreatedAtRoute(string.Empty, new { Id = result.Data!.Id }, result);
    }
}