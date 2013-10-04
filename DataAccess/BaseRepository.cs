using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        internal readonly DataContext Context;

        protected abstract DbSet<TEntity> Set { get; }

        protected virtual IQueryable<TEntity> Queryable
        {
            get
            {
                return Set;
            }
        }

        protected BaseRepository(DataContext context)
        {
            Context = context;
        }

        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            Set.Remove(entity);
        }

        public TEntity this[long id]
        {
            get { return Set.Find(id); }
            set
            {
                var entity = this[id];
                if (entity == null)
                {
                    Set.Add(value);
                }
                else
                {
                    Context.Entry(entity).CurrentValues.SetValues(value);
                }
            }
        }


        public TEntity One(Func<IQueryable<TEntity>, TEntity> query)
        {
            return query(Queryable);
        }

        public IEnumerable<TEntity> Many(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
            return query(Queryable);
        }

        public Page<TResult> Page<TResult>(Func<IQueryable<TEntity>, IOrderedQueryable<TResult>> query, Paging paging)
        {
            var queryable = query(Queryable);
            var page = new Page<TResult>();
            page.Items = queryable.Skip(paging.PageSize * (paging.PageNumber - 1 )).Take(paging.PageSize).ToList();
            page.Total = queryable.Count();
            return page;
        }
    }
}
