using System.Web.Mvc;

namespace Svitla.MovieService.MvcControllers
{
    public class MovieController : Controller
    {
        [HttpGet]
        public ViewResult List()
        {
            return View();
        }
    }
}
