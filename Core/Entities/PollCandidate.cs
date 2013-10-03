using System.Collections.Generic;

namespace Svitla.MovieService.Core.Entities
{
    public class PollCandidate : Entity
    {
        public Movie Movie { get; set; }
        public ICollection<User> Voters { get; set; }
    }
}
