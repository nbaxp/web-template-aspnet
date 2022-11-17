using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    T? Find(Guid id);

    void Add(T entity);

    void Remove(T entity);

    IQueryable<T> Set();

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
