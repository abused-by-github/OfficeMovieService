using System.Configuration;
using System.Web.Mvc;

namespace Svitla.MovieService.MvcControllers
{
    public class MovieController : Controller
    {
        [HttpGet]
        public ViewResult List()
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
