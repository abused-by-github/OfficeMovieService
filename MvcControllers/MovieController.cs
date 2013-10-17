using System.Web.Mvc;
using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.MvcControllers
{
    public class MovieController : BaseController
    {
        public MovieController(PresentationContext presentationContext) : base(presentationContext) { }

        [HttpGet]
        [return: Log(Verbosity.Full)]
        public virtual ViewResult List()
        {
            if (Session["ViewError"] != null)
            {
                ViewBag.LastError = Session["ViewError"].ToString();
                Session["ViewError"] = null;
            }
            return View();
        }
    }
}
