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
        private readonly IUserFacade userFacade;

        public MovieController(IMovieFacade movieFacade, IUserFacade userFacade)
        {
            this.movieFacade = movieFacade;
            this.userFacade = userFacade;
        }

        [HttpPost]
        public ResponseObject<Page<Movie>> List(Paging paging)
        {
            return Response(movieFacade.FindMovies(paging));
        }

        [HttpPost]
        [Authorize]
        public EmptyResponseObject Save(Movie movie)
        {
            if (movie == null || string.IsNullOrEmpty(movie.Name))
                return new EmptyResponseObject(false, "Name is required field");

            if (movie.User == null)
            {
                var currentUser = userFacade.GetByEmail(User.Identity.Name);
                movie.User = currentUser;
            }

            movieFacade.SaveMovie(movie);
            return new EmptyResponseObject(true, "Movie saved");
        }
        
        [HttpPost]
        [Authorize]
        public EmptyResponseObject Delete([FromBody]long id)
        {
            movieFacade.DeleteMovie(id);
            return new EmptyResponseObject(true, "Movie deleted");
        }
    }
}
