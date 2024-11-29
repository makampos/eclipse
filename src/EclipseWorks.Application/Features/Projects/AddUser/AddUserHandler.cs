using EclipseWorks.Application.Specifications;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.AddUser;

public class AddUserHandler : IRequestHandler<AddUserCommand, ResultResponse<AddUserResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<AddUserHandler> _logger;

    public AddUserHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<AddUserHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<AddUserResult>> Handle(AddUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {AddUserHandler} triggered to handle {AddUserCommand}",
            nameof(AddUserHandler), command);

        var project = await _eclipseUnitOfWork.ProjectRepository.GetByIdIncludeAsync
            (command.ProjectId, query => query.Include(p => p.ProjectUsers),
                cancellationToken);

        if (project is null)
        {
            _logger.LogError("Project with id {ProjectId} not found.", command.ProjectId);
            return ResultResponse<AddUserResult>.FailureResult($"Project with id {command.ProjectId} not found");
        }

        var userExistsSpecification = new UserExistsInProjectSpecification(command.UserId, command.ProjectId);

        if (userExistsSpecification.IsSatisfiedBy(project))
        {
            _logger.LogError("User with id {UserId} already exists in project with id {ProjectId}.", command.UserId, command.ProjectId);
            return ResultResponse<AddUserResult>.FailureResult($"User with id {command.UserId} already exists in project with id {command.ProjectId}");
        }

        var projectUser = ProjectUser.Create(command.UserId, command.ProjectId);
        project.AddProjectUser(projectUser);

        await _eclipseUnitOfWork.ProjectRepository.UpdateAsync(project, cancellationToken);
        await _eclipseUnitOfWork.SaveChangesAsync(cancellationToken);
        var result = AddUserResult.Create("User added to project successfully");

        return ResultResponse<AddUserResult>.SuccessResult(result);
    }
}