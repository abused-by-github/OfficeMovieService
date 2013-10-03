using System.Configuration;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.Mvc;
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
        public readonly System.Web.Mvc.IDependencyResolver MvcDependencyResolver;

        public MovieServiceApplicationContainer()
        {
            var builder = new ContainerBuilder();

            registerDataAccess(builder);
            registerDomain(builder);
            registerWebApi(builder);

            IContainer autofac = builder.Build();

            DependencyResolver = new AutofacWebApiDependencyResolver(autofac);


            var container = BuildMvcContainer();
            MvcDependencyResolver = new AutofacDependencyResolver(container);
        }

        private static IContainer BuildMvcContainer()
        {
            var builder = new ContainerBuilder();
            registerDataAccess(builder);
            registerDomain(builder);
            registerMvcControllers(builder);
            var container = builder.Build();
            return container;
        }

        private static void registerMvcControllers(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof (MvcControllers.AccountController).Assembly);
        }

        private static void registerDataAccess(ContainerBuilder builder)
        {
            builder.Register(c => new DataContext(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString)).InstancePerApiRequest();

            builder.RegisterType<MovieRepository>().As<IMovieRepository>();
            builder.RegisterType<PollRepository>().As<IPollRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
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
