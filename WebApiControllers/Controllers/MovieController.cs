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
        public EmptyResponseObject Add(Movie movie)
        {
            return new EmptyResponseObject(false, "Error");
        }
    }
}
