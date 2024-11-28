using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EclipseWorks.Application.Features.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, ResultResponse<CreateUserResult>>
{
    private readonly IEclipseUnitOfWork _eclipseUnitOfWork;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(IEclipseUnitOfWork eclipseUnitOfWork, ILogger<CreateUserHandler> logger)
    {
        _eclipseUnitOfWork = eclipseUnitOfWork;
        _logger = logger;
    }

    public async Task<ResultResponse<CreateUserResult>> Handle(CreateUserCommand command, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {CreateUserHandler} triggered to handle {CreateUserCommand}",
            nameof(CreateUserHandler), command);

        var user = command.MapToEntity();
        await _eclipseUnitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _eclipseUnitOfWork.SaveChangesAsync(cancellationToken);

        var result = user.MapToCreateUserResult();
        return ResultResponse<CreateUserResult>.SuccessResult(result);
    }
}