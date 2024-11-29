namespace EclipseWorks.Domain.Interfaces.Abstractions;

public interface ISpecification<T> where T : class
{
    bool IsSatisfiedBy(T item);
}