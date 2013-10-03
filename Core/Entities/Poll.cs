using System;
using System.Collections.Generic;

namespace Svitla.MovieService.Core.Entities
{
    public class Poll : Entity
    {
        /// <summary>
        /// Date when poll was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when poll will be closed.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Date when movie which has won will be viewed.
        /// </summary>
        public DateTime ViewDate { get; set; }

        public User Owner { get; set; }
        public ICollection<PollCandidate> Candidates { get; set; }
    }
}
