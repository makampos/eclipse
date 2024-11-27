using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Infrastructure.Implementations;
using Task = EclipseWorks.Domain.Models.Task;

namespace EclipseWorks.Infrastructure.Repositories;

public class TaskRepository
    (ApplicationDbContext applicationDbContext) : Repository<Task>(applicationDbContext), ITaskRepository
{

}