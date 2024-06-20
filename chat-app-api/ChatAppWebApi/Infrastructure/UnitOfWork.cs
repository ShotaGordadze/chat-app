using Infrastructure.Database;
using Infrastructure.Database.Abstraction;

namespace Infrastructure;

public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken);

    void Attach<TEntity>(TEntity entity) where TEntity : class;
}

public class UnitOfWork : IUnitOfWork
{
    private readonly MessagesDbContext _dbContext;

    public UnitOfWork(MessagesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Attach<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Attach(entity);
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        var entities = _dbContext.ChangeTracker.Entries<Entity>()
                                 .Select(x => x.Entity)
                                 .ToList();

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
