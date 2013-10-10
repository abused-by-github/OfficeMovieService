using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
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

        //Prevent creating proxy
        public sealed override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(controllerContext, cancellationToken);
        }
    }
}
