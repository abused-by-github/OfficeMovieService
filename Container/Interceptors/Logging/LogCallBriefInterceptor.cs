using MovieService.Core.Logging;

namespace MovieService.Container.Interceptors.Logging
{
    public class LogCallBriefInterceptor : LogCallInterceptor
    {
        public LogCallBriefInterceptor() : base(Verbosity.Default) { }
    }
}
