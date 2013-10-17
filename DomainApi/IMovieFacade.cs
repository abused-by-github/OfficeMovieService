using System.Collections.Generic;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi.DataObjects;

namespace Svitla.MovieService.DomainApi
{
    public interface IMovieFacade
    {
        void SaveMovie(Movie movie);
        void DeleteMovie(long id);
        Movie LoadById(long id);
        Page<VoteableMovie> FindMovies(Paging paging, Poll poll);
        List<Movie> FindMoviesForPoll(long pollId);
    }
}
