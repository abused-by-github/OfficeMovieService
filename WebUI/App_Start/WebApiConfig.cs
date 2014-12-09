using System.Web.Http;
using Newtonsoft.Json.Converters;

namespace MovieService.WebUI.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
        }
    }
}
