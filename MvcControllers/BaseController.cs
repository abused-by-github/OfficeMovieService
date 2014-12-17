using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieService.MvcControllers
{
    public abstract class BaseController : Controller
    {
        public readonly PresentationContext PresentationContext;

        protected BaseController(PresentationContext presentationContext)
        {
            PresentationContext = presentationContext;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.BaseUrl = GetBaseUrl();
        }

        protected string GetBaseUrl()
        {
            var request = System.Web.HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (!string.IsNullOrWhiteSpace(appUrl) && appUrl.LastOrDefault() != '/')
            {
                appUrl += "/";
            }

            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
        }
    }
}
