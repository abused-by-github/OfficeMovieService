using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Web.Http.Filters;
using MovieService.Core.Helpers;
using MovieService.WebApi.Dto;

namespace MovieService.WebApi.Filters
{
    class ErrorHandlerAttribute : ExceptionFilterAttribute
    {
        private const string DefaultErrorMessage = "Unknown Error.";

        public override void OnException(HttpActionExecutedContext context)
        {
            var resources = new ResourceManager("Resources.ErrorMessages", Assembly.Load("App_GlobalResources"));
            var errorMessage = resources.GetString(context.Exception.GetType().Name) ?? DefaultErrorMessage;
            errorMessage = errorMessage.FormatWith(context.Exception);
            var error = new EmptyResponseObject(false, errorMessage);
            context.Response = context.Request.CreateResponse(HttpStatusCode.OK, error);
        }
    }
}
