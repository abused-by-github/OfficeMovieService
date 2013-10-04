using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Svitla.MovieService.WebApi.Dto;

namespace Svitla.MovieService.WebApi.Filters
{
    class ErrorHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var error = new EmptyResponseObject(false, context.Exception.Message);
            context.Response = context.Request.CreateResponse(HttpStatusCode.OK, error);
        }
    }
}
