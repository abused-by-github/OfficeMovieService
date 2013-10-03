using System.Web.Mvc;

namespace Svitla.MovieService.WebUI.Controllers
{
    public class PollController : Controller
    {
        [HttpGet]
        public ViewResult Add()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit()
        {
            return View("Add");
        }
    }
}
