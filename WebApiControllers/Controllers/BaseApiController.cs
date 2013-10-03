using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected ResponseObject<TData> Response<TData>(TData data)
        {
            return new ResponseObject<TData>(true, "", data);
        }

        protected EmptyResponseObject Response()
        {
            return new EmptyResponseObject(true, "");
        }
    }
}
