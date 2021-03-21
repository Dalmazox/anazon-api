using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Anazon.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List();
        bool Store(T entity);
        bool Update(T entity);
        bool Remove(T entity);
        T Find(Guid id);
        T Find(Expression<Func<T, bool>> exp);
    }
}
