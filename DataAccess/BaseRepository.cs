using System;
using System.Collections.Generic;
using System.Data;
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

        protected BaseRepository(string connectionString)
        {
            Context = new DataContext(connectionString);
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
            return query(Set);
        }

        public IEnumerable<TEntity> Many(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
            return query(Set);
        }

        public Page<TEntity> Page(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> query, Paging paging)
        {
            var queryable = query(Set);
            var page = new Page<TEntity>();
            page.Items = queryable.Skip(paging.PageSize * (paging.PageNumber - 1 )).Take(paging.PageSize).ToList();
            page.Total = queryable.Count();
            return page;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }
    }
}
