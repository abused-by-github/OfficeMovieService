using System;

namespace Svitla.MovieService.Core.Exceptions
{
    public class BaseMovieException : Exception
    {
        public BaseMovieException() { }
        public BaseMovieException(string message) : base(message) { }
        public BaseMovieException(string message, Exception innerException) : base(message, innerException) { }
    }
}
