using System.Configuration;
using System.Linq;
using System.Web.Http;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi;
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
            var leftVotes = currentUser == null || currentPoll == null ? int.MaxValue : GetLeftVotes(currentUser, currentPoll.Id);

            var dto = new
            {
                Items = movies.Items.Select(m => new
                {
                    m.IsVoted,
                    m.Movie.Name,
                    m.Movie.Id,
                    m.Movie.Url,
                    m.UserName,
                    m.Movie.ImageUrl,
                    IsOwner = m.UserName == currentUser.Get(u => u.Name)
                }).ToList(),
                movies.Total,
                leftVotes
            };

            return Response<object>(dto);
        }

        private static int GetLeftVotes(User currentUser, long currentPollID)
        {
            var maxVotesStr = ConfigurationManager.AppSettings["VotesLimit"];
            int maxVotes;
            int leftVotes = int.MaxValue;
            if (int.TryParse(maxVotesStr, out maxVotes) && maxVotes > 0)
            {
                var alreadyVoted = currentUser.Votes.Count(v => v.PollId == currentPollID);
                leftVotes = maxVotes - alreadyVoted;
            }
            return leftVotes;
        }

        [HttpPost]
        [Authorize]
        public EmptyResponseObject Save(Movie movie)
        {
            if (movie == null)
                return new EmptyResponseObject(true, "Movie is empty");
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
