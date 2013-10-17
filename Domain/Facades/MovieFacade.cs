using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.Entities.Security;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.DataObjects;

namespace Svitla.MovieService.Domain.Facades
{
    public class MovieFacade : BaseFacade, IMovieFacade
    {
        private readonly IMovieRepository movies;

        public MovieFacade(DomainContext domainContext, IUnitOfWork unitOfWork, IMovieRepository movies)
            : base(unitOfWork, domainContext)
        {
            this.movies = movies;
        }

        public virtual void SaveMovie(Movie movie)
        {
            movie.Validate();
            var existedMovie = movies[movie.Id];
            if (existedMovie == null || existedMovie.User == null)
            {
                movie.User = DomainContext.CurrentUser;
            }
            else if (!existedMovie.CanBeEditedBy(DomainContext.CurrentUser))
            {
                throw new AuthenticationException("Authentication error");
            }

            movies[movie.Id] = movie;
            UnitOfWork.Commit();
        }

        public virtual Movie LoadById(long id)
        {
            return movies.One(q => q.FirstOrDefault(m => m.Id == id));
        }

        public virtual Page<VoteableMovie> FindMovies(Paging paging, Poll poll)
        {
            var currentUserId = DomainContext.CurrentUser.Get(u => u.Id);
            var pollId = poll.Get(p => p.Id);

            var result = movies.Page(q => q
                .Select(m => new VoteableMovie
                {
                    Movie = m,
                    IsVoted = m.Votes.Any(v => v.UserId == currentUserId && v.PollId == pollId),
                    UserName = m.User.Name,
                    IsOwner = m.User.Id == currentUserId
                })
                .OrderByDescending(m => m.Movie.ModifiedDate)
                , paging);
            if (DomainContext.CurrentUser.HasPermission(Permissions.EditOthersMovies))
            {
                result.Items.ForEach(m => m.IsOwner = true);
            }
            return result;
        }

        public virtual List<Movie> FindMoviesForPoll(long pollId)
        {
            return movies.Many(q => q.Where(m => m.Votes.Any(v => v.PollId == pollId))).
                OrderByDescending(m => m.Votes.Count(v => v.PollId == pollId)).ToList();
        }

        public virtual void DeleteMovie(long id)
        {
            var movie = movies[id];
            if (movie != null)
            {
                if (!movie.CanBeEditedBy(DomainContext.CurrentUser))
                    throw new AuthenticationException("You can delete only own movies");
                movies.Remove(movie);
                UnitOfWork.Commit();
            }
        }
    }
}
