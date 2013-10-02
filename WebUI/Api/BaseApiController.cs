using System.Web.Http;
using Svitla.MovieService.WebUI.Models;

namespace Svitla.MovieService.WebUI.Api
{
    public abstract class BaseApiController : ApiController
    {
        protected ResponseObject Response(object data)
        {
            return new ResponseObject(true, "", data);
        }
    }
}
