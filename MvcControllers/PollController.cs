using System.Web.Mvc;

namespace Svitla.MovieService.MvcControllers
{
    public class PollController : Controller
    {
        [HttpGet]
        [Authorize]
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
        [Authorize]
        public ViewResult Edit()
        {
            return View("Add");
        }
    }
}
