using System.Linq.Expressions;

namespace SHG.Infrastructure.Repositories.Abstraction;

public interface IRepository<T> where T : class
{
    Task<T> Find(int id);

    IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);

    Task Store(T document);

    void Delete(T document);
}
