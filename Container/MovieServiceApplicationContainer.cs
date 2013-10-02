using System.Configuration;
using Autofac;
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

        public MovieServiceApplicationContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new MovieRepository(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IMovieRepository>();
            builder.RegisterType<MovieFacade>()
                .As<IMovieFacade>();

            autofac = builder.Build();
        }

        public TComponent GetComponent<TComponent>()
        {
            return autofac.Resolve<TComponent>();
        }
    }
}
