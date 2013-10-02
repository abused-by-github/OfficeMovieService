using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Domain.Facades
{
    public class MovieFacade : IMovieFacade
    {
        private readonly IMovieRepository movies;

        public MovieFacade(IMovieRepository movies)
        {
            this.movies = movies;
        }

        public void SaveMovie(Movie movie)
        {
            movies.Add(movie);
        }

        public Movie LoadById(long id)
        {
            return movies.One(q => q.FirstOrDefault(m => m.Id == id));
        }

        public Page<Movie> FindMovies(Paging paging)
        {
            return movies.Page(q => q.OrderBy(m => m.Id), paging);
        }
    }
}
