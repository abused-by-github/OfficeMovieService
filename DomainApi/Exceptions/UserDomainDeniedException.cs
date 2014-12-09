using MovieService.Core.Exceptions;

namespace MovieService.DomainApi.Exceptions
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
