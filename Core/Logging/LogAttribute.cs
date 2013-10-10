using System;

namespace Svitla.MovieService.Core.Logging
{
    public sealed class LogAttribute : Attribute
    {
        public readonly Verbosity Verbosity;

        public LogAttribute(Verbosity verbosity)
        {
            Verbosity = verbosity;
        }
    }
}
