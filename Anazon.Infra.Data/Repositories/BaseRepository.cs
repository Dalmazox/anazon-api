using Anazon.Domain.Entities.Base;
using Anazon.Domain.Interfaces.Repositories;
using Anazon.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Anazon.Infra.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AnazonContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(AnazonContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> List()
        {
            return _dbSet.ToList();
        }

        public bool Remove(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }

        public bool Store(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;

            return _context.SaveChanges() > 0;
        }

        public T Find(Guid id)
        {
            return _dbSet.Find(id);
        }

        public T Find(Expression<Func<T, bool>> exp)
        {
            return _dbSet.FirstOrDefault(exp);
        }
    }
}
