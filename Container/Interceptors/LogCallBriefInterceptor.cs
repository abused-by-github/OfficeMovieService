using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Container.Interceptors
{
    public class LogCallBriefInterceptor : LogCallInterceptor
    {
        public LogCallBriefInterceptor() : base(Verbosity.Default) { }
    }
}
