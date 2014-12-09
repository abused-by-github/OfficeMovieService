using System;
using System.Collections.Generic;
using System.Linq;
using MovieService.Core.Entities;
using MovieService.Core.ValueObjects;

namespace MovieService.DataAccessApi
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        TEntity this[long id] { get; set; }
        TEntity One(Func<IQueryable<TEntity>, TEntity> query);
        List<TEntity> Many(Func<IQueryable<TEntity>, IQueryable<TEntity>> query);
        Page<TResult> Page<TResult> (Func<IQueryable<TEntity>, IOrderedQueryable<TResult>> query, Paging paging);
    }
}
