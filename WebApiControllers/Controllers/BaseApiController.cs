using System.Web.Http;
using Svitla.MovieService.WebApi.Dto;
using Svitla.MovieService.WebApi.Filters;

namespace Svitla.MovieService.WebApi.Controllers
{
    [ErrorHandler]
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
