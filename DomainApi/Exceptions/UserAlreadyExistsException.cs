using MovieService.Core.Exceptions;

namespace MovieService.DomainApi.Exceptions
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
