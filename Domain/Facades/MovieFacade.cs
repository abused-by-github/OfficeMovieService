using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.DataObjects;

namespace Svitla.MovieService.Domain.Facades
{
    public class MovieFacade : BaseFacade, IMovieFacade
    {
        private readonly IMovieRepository movies;

        public MovieFacade(IUnitOfWork unitOfWork, IMovieRepository movies)
            : base(unitOfWork)
        {
            this.movies = movies;
        }

        public void SaveMovie(Movie movie)
        {
            movies[movie.Id] = movie;
            UnitOfWork.Commit();
        }

        public Movie LoadById(long id)
        {
            return movies.One(q => q.FirstOrDefault(m => m.Id == id));
        }

        public Page<VoteableMovie> FindMovies(Paging paging, User user, Poll poll)
        {
            return movies.Page(q => q
                .Select(m => new VoteableMovie
                {
                    Movie = m,
                    IsVoted = m.Votes.Any(v => v.UserId == user.Id && v.PollId == poll.Id)
                })
                .OrderBy(m => m.Movie.Id)
                , paging);
        }

        public void DeleteMovie(long id)
        {
            var movie = movies[id];
            if (movie != null)
            {
                movies.Remove(movie);
                UnitOfWork.Commit();
            }
        }
    }
}
