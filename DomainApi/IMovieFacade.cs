using System.Collections.Generic;
using MovieService.Core.Entities;
using MovieService.Core.ValueObjects;
using MovieService.DomainApi.DataObjects;

namespace MovieService.DomainApi
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
