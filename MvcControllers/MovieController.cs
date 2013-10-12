using System.Web.Mvc;
using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.MvcControllers
{
    public class MovieController : Controller
    {
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
