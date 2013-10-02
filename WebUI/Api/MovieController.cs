using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebUI.Models;

namespace Svitla.MovieService.WebUI.Api
{
    public class MovieController : BaseApiController
    {
        private readonly IMovieFacade movieFacade;

        public MovieController(IMovieFacade movieFacade)
        {
            this.movieFacade = movieFacade;
        }

        [HttpPost]
        public ResponseObject List(Paging paging)
        {
            return Response(movieFacade.FindMovies(paging));
        }
        
        [HttpPost]
        public ResponseObject Add()
        {
            return null;
        }
    }
}
