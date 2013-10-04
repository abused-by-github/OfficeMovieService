using System.Collections.Generic;

namespace Svitla.MovieService.Core.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
