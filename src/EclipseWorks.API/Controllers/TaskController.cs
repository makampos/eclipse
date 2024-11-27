using EclipseWorks.API.Requests.Tasks;
using EclipseWorks.Application.Features.Tasks.CreateTask;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.API.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskController> _logger;

    public TaskController(IMediator mediator, ILogger<TaskController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType<ResultResponse<CreateTaskResult>>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskRequest request)
    {
        _logger.LogInformation("Controller {TaskController} triggered to handle {CreateTaskRequest}",
            nameof(TaskController), request);
        var command = request.ToCreateTaskCommand();
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        return CreatedAtRoute(string.Empty, new { id = result.Data!.Id }, result);
    }
}