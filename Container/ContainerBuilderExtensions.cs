using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy2;
using Svitla.MovieService.Container.Interceptors;

namespace Svitla.MovieService.Container
{
    static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<TImplementation, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterWithBriefCallLog<TImplementation, TInterface>(this ContainerBuilder builder)
        {
            var result = builder.RegisterType<TImplementation>()
                .As<TInterface>()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(LogCallBriefInterceptor));
            builder.Register(c => new LogCallBriefInterceptor());
            return result;
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
