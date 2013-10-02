using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;

namespace Svitla.MovieService.DomainApi
{
    public interface IMovieFacade
    {
        void SaveMovie(Movie movie);
        Movie LoadById(long id);
        Page<Movie> FindMovies(Paging paging);
    }
}
