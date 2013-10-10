using Svitla.MovieService.Core.Exceptions;

namespace Svitla.MovieService.DomainApi.Exceptions
{
    public class UserAlreadyExistsException : BaseMovieException
    {
        public string Email { get; private set; }

        public UserAlreadyExistsException(string email)
        {
            this.Email = email;
        }
    }
}
