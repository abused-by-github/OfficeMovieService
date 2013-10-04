using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.DataObjects;

namespace Svitla.MovieService.Domain.Facades
{
    public class MovieFacade : BaseFacade, IMovieFacade
    {
        private readonly IMovieRepository movies;

        public MovieFacade(IDomainContext domainContext, IUnitOfWork unitOfWork, IMovieRepository movies)
            : base(unitOfWork, domainContext)
        {
            this.movies = movies;
        }

        public void SaveMovie(Movie movie)
        {
            if (movie == null || string.IsNullOrEmpty(movie.Name))
                throw new ArgumentException("Name is required field");

            var existedMovie = movies[movie.Id];
            if (existedMovie == null || existedMovie.User == null)
            {
                movie.User = DomainContext.CurrentUser;
            }
            else if (existedMovie.User.Name != DomainContext.CurrentUser.Name)
            {
                throw new AuthenticationException("Authentication error");
            }

            movies[movie.Id] = movie;
            UnitOfWork.Commit();
        }

        public Movie LoadById(long id)
        {
            return movies.One(q => q.FirstOrDefault(m => m.Id == id));
        }

        public Page<VoteableMovie> FindMovies(Paging paging, User user, Poll poll)
        {
            var userId = user.Get(u => u.Id);
            var pollId = poll.Get(p => p.Id);
            return movies.Page(q => q
                .Select(m => new VoteableMovie
                {
                    Movie = m,
                    IsVoted = m.Votes.Any(v => v.UserId == userId && v.PollId == pollId),
                    UserName = m.User.Name
                })
                .OrderBy(m => m.Movie.Id)
                , paging);
        }
        
        public List<Movie> FindMoviesForPoll(long pollId)
        {
            return movies.Many(q => q.Where(m => m.Votes.Any(v => v.PollId == pollId))).
                OrderByDescending(m => m.Votes.Count(v => v.PollId == pollId)).ToList();
        }

        public void DeleteMovie(long id)
        {
            var movie = movies[id];
            if (movie != null)
            {
                if (movie.User.Id != DomainContext.CurrentUser.Id)
                    throw new AuthenticationException("You can delete only own movies");
                movies.Remove(movie);
                UnitOfWork.Commit();
            }
        }
    }
}
