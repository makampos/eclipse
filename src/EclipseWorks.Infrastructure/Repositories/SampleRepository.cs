using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Domain.Models;
using EclipseWorks.Infrastructure.Implementations;

namespace EclipseWorks.Infrastructure.Repositories;

public class SampleRepository
    (ApplicationDbContext applicationDbContext) : Repository<SampleModel>(applicationDbContext), ISampleRepository
{

}