using System.Web.Mvc;

namespace Svitla.MovieService.WebUI.Controllers
{
    [Authorize]
    public class PollController : Controller
    {
        [HttpGet]
        public ViewResult Add()
        {
            return View();
        }
        [HttpGet]
        public ViewResult Info()
        {
            return View();
        }
        [HttpGet]
        public ViewResult Edit()
        {
            return View("Add");
        }    }
}
