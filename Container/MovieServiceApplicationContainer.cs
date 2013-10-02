using System.Configuration;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Svitla.MovieService.DataAccess;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.Facades;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Container
{
    public sealed class MovieServiceApplicationContainer
    {
        private const string ConnectionString = "ConnectionString";

        private readonly IContainer autofac;

        public MovieServiceApplicationContainer(Assembly webApiAssembly)
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new MovieRepository(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IMovieRepository>();
            builder.RegisterType<MovieFacade>()
                .As<IMovieFacade>();

            if (webApiAssembly != null)
            {
                builder.RegisterApiControllers(webApiAssembly);
            }

            autofac = builder.Build();

            if (webApiAssembly != null)
            {
                var resolver = new AutofacWebApiDependencyResolver(autofac);
                GlobalConfiguration.Configuration.DependencyResolver = resolver;
            }
        }

        public TComponent GetComponent<TComponent>()
        {
            return autofac.Resolve<TComponent>();
        }
    }
}
