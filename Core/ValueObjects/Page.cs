using System.Collections.Generic;

namespace Svitla.MovieService.Core.ValueObjects
{
    public class Page<TEntity>
    {
        public IEnumerable<TEntity> Items { get; set; }
        public long Total { get; set; }
    }
}
