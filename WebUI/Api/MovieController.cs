using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebUI.Models;

namespace Svitla.MovieService.WebUI.Api
{
    public class MovieController : ApiController
    {
        private readonly IMovieFacade movieFacade;

        public MovieController(IMovieFacade movieFacade)
        {
            this.movieFacade = movieFacade;
        }

        [HttpPost]
        public ResponseObject List()
        {
            return null;
        }
        
        [HttpPost]
        public ResponseObject Add()
        {
            return movieFacade.FindMovies(new Paging(10, 1)).Items.ToList();
        }
    }
}
