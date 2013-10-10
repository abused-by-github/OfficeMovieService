using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Container.Interceptors
{
    public class LogCallVerboseInterceptor : LogCallInterceptor
    {
        public LogCallVerboseInterceptor() : base(Verbosity.Full) { }
    }
}
