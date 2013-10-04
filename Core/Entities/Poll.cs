using System;
using System.Collections.Generic;
using System.Linq;

namespace Svitla.MovieService.Core.Entities
{
    public class Poll : Entity
    {
        public string Name { get; set; }

        /// <summary>
        /// Date when poll was created.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Date when poll will be closed.
        /// </summary>
        public DateTimeOffset ExpirationDate { get; set; }

        /// <summary>
        /// Date when movie which has won will be viewed.
        /// </summary>
        public DateTimeOffset ViewDate { get; set; }

        public User Owner { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public bool IsVoteable
        {
            get
            {
                return ExpirationDate > DateTime.Now;
            }
        }

        public Movie Winner
        {
            get
            {
                return IsVoteable ? null : Votes.GroupBy(v => v.Movie).OrderBy(g => g.Count()).First().Key;
            }
        }

        public Poll()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
