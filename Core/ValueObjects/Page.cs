using System.Collections.Generic;

namespace MovieService.Core.ValueObjects
{
    public class Page<TEntity>
    {
        public List<TEntity> Items { get; set; }
        public long Total { get; set; }
    }
}
