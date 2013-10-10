using Svitla.MovieService.Core.Exceptions;

namespace Svitla.MovieService.DomainApi.Exceptions
{
    public class UserDomainDeniedException : BaseMovieException
    {
        public string AllowedDomain { get; private set; }

        public UserDomainDeniedException(string allowedDomain)
        {
            AllowedDomain = allowedDomain;
        }
    }
}
