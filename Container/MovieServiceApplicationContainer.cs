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

            registerDataAccess(builder);
            registerDomain(builder);
            registerWebApi(builder);

            IContainer autofac = builder.Build();

            DependencyResolver = new AutofacWebApiDependencyResolver(autofac);
        }

        private static void registerDataAccess(ContainerBuilder builder)
        {
            builder.Register(c => new MovieRepository(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IMovieRepository>();
            builder.Register(c => new PollRepository(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IPollRepository>();
            builder.Register(c => new UserRepository(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IUserRepository>();
        }

        private static void registerDomain(ContainerBuilder builder)
        {
            builder.RegisterType<MovieFacade>()
                .As<IMovieFacade>();
            builder.RegisterType<PollFacade>()
                .As<IPollFacade>();
            builder.RegisterType<UserFacade>()
                .As<IUserFacade>();
        }

        private static void registerWebApi(ContainerBuilder builder)
        {
            builder.RegisterType<MovieController>();
            builder.RegisterType<PollController>();
        }
    }
}
