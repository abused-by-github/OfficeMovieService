using System.Web.Mvc;

namespace Svitla.MovieService.WebUI.Controllers
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
