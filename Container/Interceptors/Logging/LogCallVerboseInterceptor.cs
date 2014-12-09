using MovieService.Core.Logging;

namespace MovieService.Container.Interceptors.Logging
{
    public class LogCallVerboseInterceptor : LogCallInterceptor
    {
        public LogCallVerboseInterceptor() : base(Verbosity.Full) { }
    }
}
