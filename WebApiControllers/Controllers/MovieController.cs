using System.Web.Http;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Controllers
{
    public class MovieController : BaseApiController
    {
        private readonly IMovieFacade movieFacade;

        public MovieController(IMovieFacade movieFacade)
        {
            this.movieFacade = movieFacade;
        }

        [HttpPost]
        public ResponseObject<Page<Movie>> List(Paging paging)
        {
            return Response(movieFacade.FindMovies(paging));
        }

        [HttpPost]
        public EmptyResponseObject Save(Movie movie)
        {
            if (movie == null || string.IsNullOrEmpty(movie.Name))
                return new EmptyResponseObject(false, "Name is required field");

            movieFacade.SaveMovie(movie);
            return new EmptyResponseObject(true, "Movie saved");
        }
        
        [HttpPost]
        public EmptyResponseObject Delete([FromBody]long id)
        {
            movieFacade.DeleteMovie(id);
            return new EmptyResponseObject(true, "Movie deleted");
        }
    }
}
