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
    }
}
