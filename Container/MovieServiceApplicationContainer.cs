using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.DataAccess;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.Domain.Facades;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.WebApi.Controllers;

namespace Svitla.MovieService.Container
{
    public sealed class MovieServiceApplicationContainer
    {
        private const string ConnectionString = "ConnectionString";

        public readonly IDependencyResolver WebApiDependencyResolver;
        public readonly System.Web.Mvc.IDependencyResolver MvcDependencyResolver;

        public MovieServiceApplicationContainer()
        {
            var builder = new ContainerBuilder();

            registerDataAccess(builder);
            registerDomain(builder);
            registerWebApi(builder);
            registerMvcControllers(builder);

            IContainer autofac = builder.Build();

            WebApiDependencyResolver = new AutofacWebApiDependencyResolver(autofac);
            MvcDependencyResolver = new AutofacDependencyResolver(autofac);
        }

        private static void registerMvcControllers(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof (MvcControllers.AccountController).Assembly);
        }

        private static void registerDataAccess(ContainerBuilder builder)
        {
            builder.Register(c => new DataContext(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IUnitOfWork>()
                .As<DataContext>()
                .InstancePerApiRequest();

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

            builder.Register(resolveDomainContext).As<IDomainContext>();
        }

        private static void registerWebApi(ContainerBuilder builder)
        {
            builder.RegisterType<MovieController>();
            builder.RegisterType<PollController>();
        }

        private static IDomainContext resolveDomainContext(IComponentContext context)
        {
            DomainContext result = new DomainContext();
            var email = HttpContext.Current.Get(c => c.User).Get(u => u.Identity).Get(i => i.Name);
            if (!string.IsNullOrEmpty(email))
            {
                var repo = context.Resolve<IUserRepository>();
                var user = repo.One(q => q.FirstOrDefault(u => u.Name == email));
                result.CurrentUser = user;
            }
            return result;
        }
    }
}
