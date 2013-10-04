using System.Web.Http;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Controllers
{
    [Authorize]
    public class PollController : BaseApiController
    {
        private readonly IPollFacade pollFacade;
        private readonly IUserFacade userFacade;
        private readonly IMovieFacade movieFacade;

        public PollController(IPollFacade pollFacade, IUserFacade userFacade, IMovieFacade movieFacade)
        {
            this.pollFacade = pollFacade;
            this.userFacade = userFacade;
            this.movieFacade = movieFacade;
        }

        [HttpPost]
        public ResponseObject<Poll> GetCurrent()
        {
            return Response(pollFacade.GetCurrent());
        }

        public EmptyResponseObject Save(Poll poll)
        {
            var currentUser = userFacade.GetByEmail(User.Identity.Name);
            poll.Owner = currentUser;
            pollFacade.Save(poll);
            return Response();
        }

        [HttpPost]
        public EmptyResponseObject Vote(EntityId id)
        {
            Vote(id.Id, true);
            return Response();
        }

        [HttpPost]
        public EmptyResponseObject Unvote(EntityId id)
        {
            Vote(id.Id, false);
            return Response();
        }

        private void Vote(long movieId, bool isSelected)
        {
            var currentUser = userFacade.GetByEmail(User.Identity.Name);
            var movie = movieFacade.LoadById(movieId);
            pollFacade.Vote(currentUser, movie, isSelected);
        }
    }
}
