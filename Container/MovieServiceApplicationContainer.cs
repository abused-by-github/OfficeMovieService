using System.Configuration;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using Svitla.MovieService.DataAccess;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.Facades;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebApi.Controllers;

namespace Svitla.MovieService.Container
{
    public sealed class MovieServiceApplicationContainer
    {
        private const string ConnectionString = "ConnectionString";

        public readonly IDependencyResolver DependencyResolver;

        public MovieServiceApplicationContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new MovieRepository(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IMovieRepository>();
            builder.RegisterType<MovieFacade>()
                .As<IMovieFacade>();

            builder.RegisterType<MovieController>();

            IContainer autofac = builder.Build();

            DependencyResolver = new AutofacWebApiDependencyResolver(autofac);
        }
    }
}
