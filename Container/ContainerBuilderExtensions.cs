using Autofac;
using Autofac.Extras.DynamicProxy2;
using Svitla.MovieService.Logging.Interceptors;

namespace Svitla.MovieService.Container
{
    static class ContainerBuilderExtensions
    {
        public static void RegisterWithBriefCallLog<TImplementation, TInterface>(this ContainerBuilder builder)
        {
            builder.RegisterType<TImplementation>()
                .As<TInterface>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallBriefInterceptor));
            builder.Register(c => new LogCallBriefInterceptor());
        }

        public static void RegisterWithBriefCallLog<TImplementation>(this ContainerBuilder builder)
        {
            builder.RegisterType<TImplementation>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallBriefInterceptor));
            builder.Register(c => new LogCallBriefInterceptor());
        }

        public static void RegisterWithFullCallLog<TImplementation, TInterface>(this ContainerBuilder builder)
        {
            builder.RegisterType<TImplementation>()
                .As<TInterface>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallVerboseInterceptor));
            builder.Register(c => new LogCallVerboseInterceptor());
        }

        public static void RegisterWithFullCallLog<TImplementation>(this ContainerBuilder builder)
        {
            builder.RegisterType<TImplementation>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallVerboseInterceptor));
            builder.Register(c => new LogCallVerboseInterceptor());
        }
    }
}
