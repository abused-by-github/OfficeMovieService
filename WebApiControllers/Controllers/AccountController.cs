using System.Web.Http;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Controllers
{
    [Authorize]
    public class AccountController : BaseApiController
    {
        private readonly IUserFacade userFacade;

        public AccountController(IUserFacade userFacade)
        {
            this.userFacade = userFacade;
        }

        [HttpPost]
        public virtual EmptyResponseObject Invite(User user)
        {
            userFacade.InviteFriend(user);
            return Response();
        }
    }
}
