using System;
using System.Collections.Generic;
using System.Linq;
using Svitla.MovieService.Core.Exceptions;
using Svitla.MovieService.Core.Logging;

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
        public DateTimeOffset? ExpirationDate { get; set; }

        /// <summary>
        /// Date when movie which has won will be viewed.
        /// </summary>
        public DateTimeOffset? ViewDate { get; set; }

        public bool IsActive { get; set; }

        [Log(Verbosity.Full)]
        public virtual User Owner { get; set; }

        [Log(Verbosity.Full)]
        public virtual ICollection<Vote> Votes { get; set; }

        public bool IsVoteable
        {
            get
            {
                return !ExpirationDate.HasValue || ExpirationDate > DateTime.Now;
            }
        }

        private Movie winner;

        [Log(Verbosity.Full)]
        public Movie Winner
        {
            get
            {
                if (winner != null)
                {
                    return winner;
                }
                return IsVoteable || Votes == null || Votes.Count == 0 ? null : Votes.GroupBy(v => v.Movie).OrderBy(g => g.Count()).First().Key;
            }
        }

        public Poll()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new EntityInvalidException("Name is required.");
            }

            if (Owner == null)
            {
                throw new EntityInvalidException("Owner must be specified.");
            }
        }
    }
}
