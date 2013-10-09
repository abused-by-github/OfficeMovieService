using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Logging.Interceptors
{
    public class LogCallBriefInterceptor : LogCallInterceptor
    {
        public LogCallBriefInterceptor() : base(Verbosity.Default) { }
    }
}
