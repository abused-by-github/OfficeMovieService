using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.DomainApi
{
    public interface IUserFacade
    {
        User GetByEmail(string email);
        void Save(User user);
        void InviteFriend(User user);
    }
}
