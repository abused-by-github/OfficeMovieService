using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.DataAccess;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.Domain.Facades;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.Mailing.Core;
using Svitla.MovieService.Mailing.Core.Client;
using Svitla.MovieService.Mailing.Emails;
using Svitla.MovieService.MailingApi;
using Svitla.MovieService.WebApi.Controllers;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

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
            registerMailing(builder);
            registerDomain(builder);
            registerWebApi(builder);
            registerMvcControllers(builder);

            IContainer autofac = builder.Build();

            WebApiDependencyResolver = new AutofacWebApiDependencyResolver(autofac);
            MvcDependencyResolver = new AutofacDependencyResolver(autofac);
        }

        private static void registerMvcControllers(ContainerBuilder builder)
        {
            builder.RegisterWithBriefCallLog<MvcControllers.AccountController>();
            builder.RegisterWithBriefCallLog<MvcControllers.MovieController>();
        }

        private static void registerDataAccess(ContainerBuilder builder)
        {
            builder.Register(c => new DataContext(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString))
                .As<IUnitOfWork>()
                .As<DataContext>()
                .InstancePerApiRequest();

            builder.RegisterWithBriefCallLog<MovieRepository, IMovieRepository>();
            builder.RegisterWithBriefCallLog<PollRepository, IPollRepository>();
            builder.RegisterWithBriefCallLog<UserRepository, IUserRepository>();
        }

        private static void registerDomain(ContainerBuilder builder)
        {
            builder.Register<Func<IInviteEmail>>(c =>
            {
                //One more resolve because of
                //http://stackoverflow.com/questions/5383888/autofac-registration-issue-in-release-v2-4-5-724
                var ctx = c.Resolve<IComponentContext>();
                return () => ctx.Resolve<InviteEmail>();
            });

            builder.RegisterWithBriefCallLog<MovieFacade, IMovieFacade>();
            builder.RegisterWithBriefCallLog<PollFacade, IPollFacade>();
            builder.RegisterWithBriefCallLog<UserFacade, IUserFacade>()
                .OnActivated(uf => uf.Instance.AllowedDomain = ConfigurationManager.AppSettings["AllowedDomain"]);

            builder.Register(resolveDomainContext);
        }

        private static void registerWebApi(ContainerBuilder builder)
        {
            builder.RegisterWithFullCallLog<MovieController>();
            builder.RegisterWithFullCallLog<PollController>();
            builder.RegisterWithFullCallLog<AccountController>();
        }

        private void registerMailing(ContainerBuilder builder)
        {
            builder.Register(c => resolveEmailConfig());
            builder.Register(c => resolveSmtpConfig());
            builder.RegisterWithBriefCallLog<SmtpClient, IEmailClient>();
            builder.RegisterWithBriefCallLog<InviteEmail, InviteEmail>();
        }

        private EmailConfig resolveEmailConfig()
        {
            return new EmailConfig
            {
                DefaultFrom = ConfigurationManager.AppSettings["Mail.DefaultFrom"],
                WebAppUrl = GetBaseUrl(),
            };
        }

        private SmtpConfig resolveSmtpConfig()
        {
            return new SmtpConfig
            {
                Host = ConfigurationManager.AppSettings["Mail.SmtpHost"],
                Login = ConfigurationManager.AppSettings["Mail.SmtpLogin"],
                Password = ConfigurationManager.AppSettings["Mail.SmtpPassword"],
                Port = int.Parse(ConfigurationManager.AppSettings["Mail.SmtpPort"]),
                UseSsl = bool.Parse(ConfigurationManager.AppSettings["Mail.SmtpUseSsl"])
            };
        }

        private static DomainContext resolveDomainContext(IComponentContext context)
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

        private string GetBaseUrl(string path = null)
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
