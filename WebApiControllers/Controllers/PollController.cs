using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Controllers
{
    public class PollController : BaseApiController
    {
        private readonly IPollFacade pollFacade;
        private readonly IUserFacade userFacade;

        public PollController(IPollFacade pollFacade, IUserFacade userFacade)
        {
            this.pollFacade = pollFacade;
            this.userFacade = userFacade;
        }

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
    }
}
