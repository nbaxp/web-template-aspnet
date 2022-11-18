using Microsoft.EntityFrameworkCore;
using WebTemplate.Application.Interfaces;
using WebTemplate.Application.Shared;

namespace WebTemplate.Infrastructure.Data;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbContext _efDbContext;

    public EfRepository(DbContext efDbContext)
    {
        this._efDbContext = efDbContext;
    }

    public T? Find(Guid id)
    {
        return _efDbContext.Set<T>().Find(id);
    }

    public void Add(T entity)
    {
        _efDbContext.Set<T>().Add(entity);
    }

    public void Remove(T entity)
    {
        _efDbContext.Remove(entity);
    }

    public IQueryable<T> Set()
    {
        return _efDbContext.Set<T>();
    }

    public int SaveChanges()
    {
        return _efDbContext.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return _efDbContext.SaveChangesAsync(cancellationToken);
    }
}
