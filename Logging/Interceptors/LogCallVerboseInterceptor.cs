using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Logging.Interceptors
{
    public class LogCallVerboseInterceptor : LogCallInterceptor
    {
        public LogCallVerboseInterceptor() : base(Verbosity.Full) { }
    }
}
