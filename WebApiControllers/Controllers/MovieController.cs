using System.Linq;
using System.Web.Http;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.DataObjects;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Controllers
{
    public class MovieController : BaseApiController
    {
        private readonly IMovieFacade movieFacade;
        private readonly IUserFacade userFacade;
        private readonly IPollFacade pollFacade;

        public MovieController(IMovieFacade movieFacade, IUserFacade userFacade, IPollFacade pollFacade)
        {
            this.movieFacade = movieFacade;
            this.userFacade = userFacade;
            this.pollFacade = pollFacade;
        }

        [HttpPost]
        public ResponseObject<object> List(Paging paging)
        {
            var currentUser = userFacade.GetByEmail(User.Identity.Name);
            var currentPoll = pollFacade.GetCurrent();
            var movies = movieFacade.FindMovies(paging, currentUser, currentPoll);

            var dto = new
            {
                Items = movies.Items.Select(m => new
                {
                    m.IsVoted,
                    m.Movie.Name,
                    m.Movie.Id,
                    m.Movie.Url,
                    UserName = m.Movie.User.Name
                }),
                movies.Total
            };

            return Response<object>(dto);
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
