using System;
using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Core.Entities
{
    public class Vote : IEquatable<Vote>
    {
        public long UserId { get; set; }
        public long MovieId { get; set; }
        public long PollId { get; set; }

        public bool HasNotificationBeenSent { get; set; }

        [Log(Verbosity.Full)]
        public virtual User User { get; set; }

        [Log(Verbosity.Full)]
        public virtual Poll Poll { get; set; }

        [Log(Verbosity.Full)]
        public virtual Movie Movie { get; set; }

        public bool Equals(Vote other)
        {
            var result = ReferenceEquals(null, other);
            if (!result)
            {
                var equalsById = other.MovieId == MovieId && other.PollId == PollId && other.UserId == UserId;
                var equalsByavigation = User != null && User == other.User
                    && Movie != null && Movie == other.Movie
                    && Poll != null && Poll == other.Poll;

                result = equalsById || equalsByavigation;
            }
            return result;
        }

        public override int GetHashCode()
        {
            var userId = User == null ? UserId : User.Id;
            var pollId = Poll == null ? PollId : Poll.Id;
            var movieId = Movie == null ? MovieId : Movie.Id;

            return string.Format("{0}|{1}|{2}", userId, pollId, movieId).GetHashCode();
        }
    }
}
