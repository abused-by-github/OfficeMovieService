using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Svitla.MovieService.Container;
using Svitla.MovieService.WebUI.App_Start;
using WebUI;

namespace Svitla.MovieService.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private MovieServiceApplicationContainer movieServiceApplication;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            movieServiceApplication = new MovieServiceApplicationContainer();
            GlobalConfiguration.Configuration.DependencyResolver = movieServiceApplication.DependencyResolver;
            DependencyResolver.SetResolver(movieServiceApplication.MvcDependencyResolver);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}