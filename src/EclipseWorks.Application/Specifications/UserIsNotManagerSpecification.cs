using EclipseWorks.Domain.Enum;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Models;

namespace EclipseWorks.Application.Specifications;

public class UserIsNotManagerSpecification : ISpecification<User>
{
    public bool IsSatisfiedBy(User user)
    {
        return user.Role is not Role.Manager;
    }
}