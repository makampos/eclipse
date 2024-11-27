using EclipseWorks.Domain.Interfaces.Abstractions;
using Task = EclipseWorks.Domain.Models.Task;


namespace EclipseWorks.Domain.Interfaces.Repositories;

public interface ITaskRepository : IRepository<Task>
{

}