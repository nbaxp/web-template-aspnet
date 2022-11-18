using System.Collections;
using System.Linq.Expressions;

namespace WebTemplate.Application.interfaces;

public class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>
{
    private readonly IQueryable<TEntity> _query;

    public IncludableQueryable(IQueryable<TEntity> query)
    {
        this._query = query;
    }

    public Type ElementType => _query.ElementType;

    public Expression Expression => _query.Expression;

    public IQueryProvider Provider => _query.Provider;

    public IEnumerator<TEntity> GetEnumerator()
    {
        return _query.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
