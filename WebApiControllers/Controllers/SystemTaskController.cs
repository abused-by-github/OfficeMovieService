using System.Web.Http;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.Exceptions;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Controllers
{
    public class SystemTaskController : BaseApiController
    {
        private readonly ISystemTaskFacade facade;
        private readonly AppSettings appSettings;

        public SystemTaskController(ISystemTaskFacade facade, AppSettings appSettings)
        {
            this.facade = facade;
            this.appSettings = appSettings;
        }

        [HttpGet]
        public virtual EmptyResponseObject SendEmails(string key)
        {
            if (key != appSettings.SystemTaskSecurityKey)
                throw new AccessDeniedException("Access key is invalid.");

            facade.Process();
            return Response();
        }
    }
}
