using System;
using System.Collections.Generic;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;

namespace Svitla.MovieService.DataAccessApi
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
