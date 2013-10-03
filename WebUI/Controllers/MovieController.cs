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
