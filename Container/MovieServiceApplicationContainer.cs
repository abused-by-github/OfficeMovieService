using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Svitla.MovieService.Container.Interceptors.Logging;
using Svitla.MovieService.Container.Interceptors.Security;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.Core.ValueObjects;
using Svitla.MovieService.DataAccess;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.Domain.Facades;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.Mailing.Core;
using Svitla.MovieService.Mailing.Core.Client;
using Svitla.MovieService.Mailing.Emails;
using Svitla.MovieService.MailingApi;
using Svitla.MovieService.MvcControllers;
using Svitla.MovieService.WebApi.Controllers;
using AccountController = Svitla.MovieService.WebApi.Controllers.AccountController;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;
using MovieController = Svitla.MovieService.WebApi.Controllers.MovieController;

namespace Svitla.MovieService.Container
{
    public sealed class MovieServiceApplicationContainer
    {
        private static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
        }

        private static string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public readonly IDependencyResolver WebApiDependencyResolver;
        public readonly System.Web.Mvc.IDependencyResolver MvcDependencyResolver;

        public MovieServiceApplicationContainer()
        {
            var builder = new ContainerBuilder();

            RegisterInterceptors(builder);
            RegisterDataAccess(builder);
            RegisterMailing(builder);
            RegisterDomain(builder);
            RegisterWebApi(builder);
            RegisterMvcControllers(builder);

            IContainer autofac = builder.Build();

            WebApiDependencyResolver = new AutofacWebApiDependencyResolver(autofac);
            MvcDependencyResolver = new AutofacDependencyResolver(autofac);
        }

        private static void RegisterMvcControllers(ContainerBuilder builder)
        {
            builder.Register(ResolvePresentationContext);
            builder.RegisterWithBriefCallLog<MvcControllers.AccountController>();
            builder.RegisterWithBriefCallLog<MvcControllers.MovieController>();
        }

        private static void RegisterDataAccess(ContainerBuilder builder)
        {
            builder.Register(c => new DataContext(ConnectionString))
                .As<IUnitOfWork>()
                .As<DataContext>()
                .InstancePerApiRequest();

            builder.RegisterType<MovieRepository>()
                .As<IMovieRepository>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof (LogCallBriefInterceptor));

            builder.RegisterType<PollRepository>()
                .As<IPollRepository>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallBriefInterceptor));

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallBriefInterceptor));
        }

        private static void RegisterDomain(ContainerBuilder builder)
        {
            //Invite Email factory
            builder.Register<Func<IInviteEmail>>(c =>
            {
                //One more resolve because of
                //http://stackoverflow.com/questions/5383888/autofac-registration-issue-in-release-v2-4-5-724
                var ctx = c.Resolve<IComponentContext>();
                return () => ctx.Resolve<InviteEmail>();
            });

            builder.Register(ResolveDomainContext);

            builder.RegisterType<MovieFacade>()
                .As<IMovieFacade>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof (SecureMethodInterceptor))
                .InterceptedBy(typeof (LogCallBriefInterceptor));

            builder.RegisterType<PollFacade>()
                .As<IPollFacade>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(SecureMethodInterceptor))
                .InterceptedBy(typeof(LogCallBriefInterceptor));

            builder.RegisterType<UserFacade>()
                .As<IUserFacade>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(SecureMethodInterceptor))
                .InterceptedBy(typeof(LogCallBriefInterceptor))
                .OnActivated(uf => uf.Instance.AllowedDomain = AppSettings("AllowedDomain"));
        }

        private static void RegisterWebApi(ContainerBuilder builder)
        {
            builder.Register(c => ResolveAppSettings());

            builder.RegisterType<MovieController>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallVerboseInterceptor));

            builder.RegisterType<PollController>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallVerboseInterceptor));

            builder.RegisterType<AccountController>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallVerboseInterceptor));
        }

        private void RegisterMailing(ContainerBuilder builder)
        {
            builder.Register(c => ResolveEmailConfig());
            builder.Register(c => ResolveSmtpConfig());

            builder.RegisterType<SmtpClient>()
                .As<IEmailClient>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallBriefInterceptor));

            builder.RegisterType<InviteEmail>()
                .As<InviteEmail>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallBriefInterceptor));
        }

        private static void RegisterInterceptors(ContainerBuilder builder)
        {
            builder.RegisterType<SecureMethodInterceptor>();
            builder.RegisterType<LogCallBriefInterceptor>();
            builder.RegisterType<LogCallVerboseInterceptor>();
        }

        private EmailConfig ResolveEmailConfig()
        {
            return new EmailConfig
            {
                DefaultFrom = AppSettings("Mail.DefaultFrom"),
                WebAppUrl = GetBaseUrl(),
            };
        }

        private static SmtpConfig ResolveSmtpConfig()
        {
            return new SmtpConfig
            {
                Host = AppSettings("Mail.SmtpHost"),
                Login = AppSettings("Mail.SmtpLogin"),
                Password = AppSettings("Mail.SmtpPassword"),
                Port = int.Parse(AppSettings("Mail.SmtpPort")),
                UseSsl = bool.Parse(AppSettings("Mail.SmtpUseSsl"))
            };
        }

        private static AppSettings ResolveAppSettings()
        {
            return new AppSettings
            {
                BaseTmdbUrl = AppSettings("TmdbBaseUrl")
            };
        }

        private static DomainContext ResolveDomainContext(IComponentContext context)
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

        private static PresentationContext ResolvePresentationContext(IComponentContext context)
        {
            var domain = ResolveDomainContext(context);
            return new PresentationContext { CurrentUser = domain.CurrentUser };
        }

        private static string GetBaseUrl(string path = null)
        {
            var request = HttpContext.Current.Request;
            if (request == null || request.Url == null)
                throw new Exception("Request is null");
            path = path ?? HttpRuntime.AppDomainAppVirtualPath;
            string baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, path);
            return baseUrl;
        }
    }
}
