using System.Web.Mvc;

namespace Svitla.MovieService.MvcControllers
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
    }
}
