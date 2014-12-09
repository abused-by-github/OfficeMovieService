using System;

namespace MovieService.Core.Exceptions
{
    public class EntityInvalidException : BaseMovieException
    {
        public EntityInvalidException(string message) : base(message) { }
        public EntityInvalidException() { }
        public EntityInvalidException(string message, Exception innerException) : base(message, innerException) { }
    }
}
