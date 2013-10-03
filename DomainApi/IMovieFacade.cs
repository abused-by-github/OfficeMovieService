using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;

namespace Svitla.MovieService.DomainApi
{
    public interface IMovieFacade
    {
        void SaveMovie(Movie movie);
        void DeleteMovie(long id);
        Movie LoadById(long id);
        Page<Movie> FindMovies(Paging paging);
    }
}
