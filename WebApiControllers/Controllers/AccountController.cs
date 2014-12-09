using System.Web.Http;
using MovieService.Core.Entities;
using MovieService.DomainApi;
using MovieService.WebApi.Dto;

namespace MovieService.WebApi.Controllers
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
