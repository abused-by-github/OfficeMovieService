using System;
using System.Collections.Generic;
using System.Linq;
using Svitla.MovieService.Core.Exceptions;
using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Core.Entities
{
    public class Poll : Entity
    {
        public static Func<IQueryable<Poll>, IQueryable<Poll>> WhichNeedNotifications =
            polls => polls.Where(p => p.IsActive && p.ExpirationDate < DateTime.Now && !p.HaveNotificationsBeenSent);

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

        public bool HaveNotificationsBeenSent { get; set; }

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

        [Log(Verbosity.Full)]
        public Movie Winner
        {
            get
            {
                var noWinner = IsVoteable || Votes == null || Votes.Count == 0;
                return noWinner ? null : Votes.GroupBy(v => v.Movie).OrderByDescending(g => g.Count()).First().Key;
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
