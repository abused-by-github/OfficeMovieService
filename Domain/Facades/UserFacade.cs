using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Domain.Facades
{
    public class UserFacade : IUserFacade
    {
        private readonly IUserRepository users;

        public UserFacade(IUserRepository userRepository)
        {
            users = userRepository;
        }

        public User GetByEmail(string email)
        {
            return users.One(q => q.FirstOrDefault(u => u.Name == email));
        }
    }
}
